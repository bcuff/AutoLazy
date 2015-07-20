using System;
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
            CreateFields();
            InitializeFields();
            _implMethod = _method.CopyToPrivateMethod(_method.Name + "$Impl");
            WriteInstructions();
            _method.Body.OptimizeMacros();
        }

        private void CreateFields()
        {
            var fieldAttributes = FieldAttributes.Private;
            if (_method.IsStatic) fieldAttributes |= FieldAttributes.Static;
            _valueField = new FieldDefinition(_method.Name + "$Value", fieldAttributes, _method.ReturnType);
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
            var result = new VariableDefinition(_method.ReturnType);
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
                        if (!_method.IsStatic)
                        {
                            // once for the call & once for the store field
                            il.Emit(OpCodes.Ldarg_0);
                            il.Emit(OpCodes.Ldarg_0);
                        }
                        il.EmitCall(_implMethod);
                        il.Emit(OpCodes.Stloc, result);
                        il.Emit(OpCodes.Ldloc, result);
                        il.Emit(OpCodes.Volatile);
                        il.EmitStore(_valueField);
                    }
                });
            }
            il.Emit(OpCodes.Ldloc, result);
            il.Emit(OpCodes.Ret);
        }
    }
}
