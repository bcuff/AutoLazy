using System;
using System.Diagnostics;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;
using Mono.Cecil.Rocks;
using FieldAttributes = Mono.Cecil.FieldAttributes;

namespace AutoLazy.Fody
{
    internal class DoubleCheckedLockingWeaver
    {
        readonly MethodDefinition _method;
        readonly TypeReference _objRef;
        readonly MethodReference _objCtorRef;
        TypeDefinition _valueTypeWrapper;
        FieldDefinition _valueTypeWrapperField;
        MethodDefinition _valueTypeWrapperCtor;
        MethodDefinition _implMethod;
        FieldDefinition _valueField;
        FieldDefinition _syncRootField;

        public DoubleCheckedLockingWeaver(MethodDefinition method)
        {
            _method = method;
            _objRef = method.Module.Import(typeof(object));
            _objCtorRef = method.Module.Import(new MethodReference(".ctor", method.Module.TypeSystem.Void, _objRef)
            {
                HasThis = true,
            });
        }

        public void Instrument()
        {
            if (_method.ReturnType.IsValueType)
            {
                InitializeValueTypeWrapper();
            }
            CreateFields();
            InitializeFields();
            _implMethod = _method.CopyToPrivateMethod(_method.Name + "$Impl");
            WriteInstructions();
            _method.Body.OptimizeMacros();
        }

        static int _wrapperCounter;

        private void InitializeValueTypeWrapper()
        {
            var type = _method.DeclaringType;
            var wrapperType = type.NestedTypes.FirstOrDefault(t =>
                t.Name.StartsWith("AutoLazy$")
                && t.Fields.Count == 1
                && t.Fields[0].FieldType == _method.ReturnType);
            if (wrapperType == null)
            {
                var typeAttributes = TypeAttributes.Class
                    | TypeAttributes.NestedPrivate
                    | TypeAttributes.Sealed
                    | TypeAttributes.BeforeFieldInit;
                var field = new FieldDefinition("Value", FieldAttributes.Public | FieldAttributes.Public, _method.ReturnType);
                var ctor = new MethodDefinition(".ctor", MethodAttributes.HideBySig | MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.RTSpecialName, _method.Module.TypeSystem.Void);
                var il = ctor.Body.GetILProcessor();
                il.Emit(OpCodes.Ldarg_0);
                il.Emit(OpCodes.Call, _objCtorRef);
                il.Emit(OpCodes.Ret);
                wrapperType = new TypeDefinition(type.Namespace, "AutoLazy$" + _method.ReturnType.Name + "$Wrapper" + ++_wrapperCounter, typeAttributes, _objRef)
                {
                    Methods = { ctor },
                    Fields = { field },
                };
                type.NestedTypes.Add(wrapperType);
            }
            _valueTypeWrapper = wrapperType;
            _valueTypeWrapperField = wrapperType.Fields[0];
            _valueTypeWrapperCtor = wrapperType.Methods[0];
        }

        private void CreateFields()
        {
            var fieldAttributes = FieldAttributes.Private;
            if (_method.IsStatic) fieldAttributes |= FieldAttributes.Static;
            var fieldType = _method.ReturnType.IsValueType
                ? _valueTypeWrapper
                : _method.ReturnType;
            _valueField = new FieldDefinition(_method.Name + "$Value", fieldAttributes, fieldType);
            _method.DeclaringType.Fields.Add(_valueField);

            _syncRootField = new FieldDefinition(_method.Name + "$SyncRoot", fieldAttributes | FieldAttributes.InitOnly, _objRef);
            _method.DeclaringType.Fields.Add(_syncRootField);
        }

        private void InitializeFields()
        {
            if (_method.IsStatic)
            {
                var ctor = _method.DeclaringType.GetOrCreateStaticConstructor();
                InitializeFields(ctor);
            }
            else
            {
                foreach (var ctor in _method.DeclaringType.GetConstructors().Where(c => !c.IsStatic))
                {
                    InitializeFields(ctor);
                }
            }
        }

        private void InitializeFields(MethodDefinition ctor)
        {
            var il = ctor.Body.GetILProcessor();
            var start = ctor.Body.Instructions.First();
            if (!_method.IsStatic) il.InsertBefore(start, il.Create(OpCodes.Ldarg_0));
            il.InsertBefore(start, il.Create(OpCodes.Newobj, _objCtorRef));
            il.InsertBefore(start, il.CreateStore(_syncRootField));
        }

        private void WriteInstructions()
        {
            _method.Body.Variables.Clear();
            _method.Body.Instructions.Clear();
            _method.Body.ExceptionHandlers.Clear();
            var il = _method.Body.GetILProcessor();
            var result = new VariableDefinition(_valueField.FieldType);
            _method.Body.InitLocals = true;
            _method.Body.Variables.Add(result);
            if (!_method.IsStatic) il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Volatile);
            il.EmitLoad(_valueField);
            il.Emit(OpCodes.Stloc, result);
            il.Emit(OpCodes.Ldloc, result);
            using (il.BranchIfTrue())
            {
                il.EmitLock(() =>
                {
                    if (!_method.IsStatic) il.Emit(OpCodes.Ldarg_0);
                    il.EmitLoad(_syncRootField);
                }, () =>
                {
                    if (!_method.IsStatic) il.Emit(OpCodes.Ldarg_0);
                    il.EmitLoad(_valueField);
                    il.Emit(OpCodes.Stloc, result);
                    il.Emit(OpCodes.Ldloc, result);
                    using (il.BranchIfTrue())
                    {
                        if (_valueTypeWrapper != null)
                        {
                            il.Emit(OpCodes.Newobj, _valueTypeWrapperCtor);
                            il.Emit(OpCodes.Dup);
                        }
                        if (!_method.IsStatic) il.Emit(OpCodes.Ldarg_0);
                        il.EmitCall(_implMethod);
                        if (_valueTypeWrapper != null)
                        {
                            il.Emit(OpCodes.Stfld, _valueTypeWrapperField);
                        }
                        il.Emit(OpCodes.Stloc, result);
                        if (!_method.IsStatic) il.Emit(OpCodes.Ldarg_0);
                        il.Emit(OpCodes.Ldloc, result);
                        il.Emit(OpCodes.Volatile);
                        il.EmitStore(_valueField);
                    }
                });
            }
            il.Emit(OpCodes.Ldloc, result);
            if (_valueTypeWrapper != null)
            {
                il.Emit(OpCodes.Ldfld, _valueTypeWrapperField);
            }
            il.Emit(OpCodes.Ret);
        }
    }
}
