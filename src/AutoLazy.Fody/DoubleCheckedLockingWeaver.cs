using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;
using Mono.Cecil.Rocks;
using FieldAttributes = Mono.Cecil.FieldAttributes;
using MethodAttributes = Mono.Cecil.MethodAttributes;
using OpCodes = Mono.Cecil.Cil.OpCodes;
using TypeAttributes = Mono.Cecil.TypeAttributes;

namespace AutoLazy.Fody
{
    internal class DoubleCheckedLockingWeaver : LazyWeaver
    {
        TypeReference _objRef;
        MethodReference _objCtorRef;
        FieldDefinition _syncRootField;
        FieldReference _syncRootFieldRef;
        TypeReference _valueWrapper;
        FieldReference _valueWrapperField;
        MethodReference _valueWrapperCtor;
        FieldDefinition _valueField;
        FieldReference _valueFieldReference;

        public DoubleCheckedLockingWeaver(MethodDefinition method, VisitorContext context)
            : base(method, context)
        {
        }

        protected override bool Validate()
        {
            var valid = true;
            if (Method.Parameters.Count > 0)
            {
                Context.LogError("[Lazy] methods may not have any parameters.", Method);
                valid = false;
            }
            return valid && base.Validate();
        }

        protected override void InitializeTypes()
        {
            base.InitializeTypes();

            _objRef = Method.Module. ImportReference ( typeof(object));
            _objCtorRef = Method.Module. ImportReference ( new MethodReference(".ctor", Method.Module.TypeSystem.Void, _objRef)
            {
                HasThis = true,
            });

            if (Method.ReturnType.IsValueType || Method.ReturnType.IsGenericParameter)
            {
                InitializeValueWrapper();
            }
        }

        static int _wrapperCounter;

        private void InitializeValueWrapper()
        {
            var type = Method.DeclaringType;
            var wrapperType = type.NestedTypes.FirstOrDefault(t =>
                t.Name.StartsWith("AutoLazy$")
                && t.Fields.Count == 1
                && t.Fields[0].FieldType == Method.ReturnType);
            if (wrapperType == null)
            {
                var typeAttributes = TypeAttributes.Class
                    | TypeAttributes.NestedPrivate
                    | TypeAttributes.Sealed
                    | TypeAttributes.BeforeFieldInit;
                wrapperType = new TypeDefinition(string.Empty, "AutoLazy$" + Method.ReturnType.Name + "$Wrapper" + ++_wrapperCounter, typeAttributes, _objRef);
                type.NestedTypes.Add(wrapperType);
                FieldDefinition field;
                if (Method.ReturnType.IsGenericParameter)
                {
                    var param = new GenericParameter("TInner", wrapperType);
                    wrapperType.GenericParameters.Add(param);
                    field = new FieldDefinition("Value", FieldAttributes.Public, param);
                    wrapperType.Fields.Add(field);
                }
                else
                {
                    field = new FieldDefinition("Value", FieldAttributes.Public, Method.ReturnType);
                    wrapperType.Fields.Add(field);
                }
                var ctor = new MethodDefinition(".ctor", MethodAttributes.HideBySig | MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.RTSpecialName, Method.Module.TypeSystem.Void);
                wrapperType.Methods.Add(ctor);
                var il = ctor.Body.GetILProcessor();
                il.Emit(OpCodes.Ldarg_0);
                il.Emit(OpCodes.Call, _objCtorRef);
                il.Emit(OpCodes.Ret);
            }
            if (Method.ReturnType.IsGenericParameter)
            {
                //_valueWrapper = wrapperType;
                _valueWrapper = new GenericInstanceType(wrapperType) { GenericArguments = { Method.ReturnType } };
                _valueWrapperCtor = new MethodReference(".ctor", Method.Module.TypeSystem.Void, _valueWrapper)
                {
                    HasThis = true
                };
                _valueWrapperField = new FieldReference("Value", Method.ReturnType, _valueWrapper);
            }
            else
            {
                _valueWrapper = wrapperType;
                _valueWrapperCtor = wrapperType.Methods[0];
                _valueWrapperField = wrapperType.Fields[0];
            }
        }

        protected override void CreateFields()
        {
            base.CreateFields();
            var fieldAttributes = FieldAttributes.Private;
            if (Method.IsStatic) fieldAttributes |= FieldAttributes.Static;

            _syncRootField = new FieldDefinition(Method.Name + "$SyncRoot", fieldAttributes | FieldAttributes.InitOnly, _objRef);
            Method.DeclaringType.Fields.Add(_syncRootField);
            if (Method.DeclaringType.HasGenericParameters)
            {
                var declaringType = Method.DeclaringType.MakeGenericInstanceType(
                    Method.DeclaringType.GenericParameters.Cast<TypeReference>().ToArray());
                _syncRootFieldRef = new FieldReference(_syncRootField.Name, _objRef, declaringType);
            }
            else
            {
                _syncRootFieldRef = _syncRootField;
            }
            
            var fieldType = _valueWrapper ?? Method.ReturnType;
            _valueFieldReference = _valueField = new FieldDefinition(Method.Name + "$Value", fieldAttributes, fieldType);
            Method.DeclaringType.Fields.Add(_valueField);
            if (Method.DeclaringType.HasGenericParameters)
            {
                var declaringType = Method.DeclaringType.MakeGenericInstanceType(Method.DeclaringType.GenericParameters.Cast<TypeReference>().ToArray());
                _valueFieldReference = new FieldReference(_valueField.Name, fieldType, declaringType);
            }
        }

        protected override void InitializeFields(MethodDefinition ctor)
        {
            var il = ctor.Body.GetILProcessor();
            var start = ctor.Body.Instructions.First();
            if (!Method.IsStatic) il.InsertBefore(start, il.Create(OpCodes.Ldarg_0));
            il.InsertBefore(start, il.Create(OpCodes.Newobj, _objCtorRef));
            il.InsertBefore(start, il.Create(Method.IsStatic ? OpCodes.Stsfld : OpCodes.Stfld, _syncRootFieldRef));
            base.InitializeFields(ctor);
        }

        protected override void WriteInstructions()
        {
            base.WriteInstructions();
            PrepForRewrite();
            var bodyInstructions = Method.Body.Instructions.ToList();
            Method.Body.Instructions.Clear();
            var il = Method.Body.GetILProcessor();
            var result = new VariableDefinition(_valueFieldReference.FieldType);
            var val = new VariableDefinition(Method.ReturnType);
            Method.Body.InitLocals = true;
            Method.Body.Variables.Add(result);
            Method.Body.Variables.Add(val);
            if (!Method.IsStatic) il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Volatile);
            il.Emit(Method.IsStatic ? OpCodes.Ldsfld : OpCodes.Ldfld, _valueFieldReference);
            il.Emit(OpCodes.Stloc, result);
            il.Emit(OpCodes.Ldloc, result);
            using (il.BranchIfTrue())
            {
                il.EmitLock(() =>
                {
                    if (!Method.IsStatic) il.Emit(OpCodes.Ldarg_0);
                    il.Emit(Method.IsStatic ? OpCodes.Ldsfld : OpCodes.Ldfld, _syncRootFieldRef);
                }, () =>
                {
                    if (!Method.IsStatic) il.Emit(OpCodes.Ldarg_0);
                    il.Emit(Method.IsStatic ? OpCodes.Ldsfld : OpCodes.Ldfld, _valueFieldReference);
                    il.Emit(OpCodes.Stloc, result);
                    il.Emit(OpCodes.Ldloc, result);
                    using (il.BranchIfTrue())
                    {
                        foreach (var instruction in bodyInstructions)
                        {
                            Method.Body.Instructions.Add(instruction);
                        }
                        if (_valueWrapper != null)
                        {
                            il.Emit(OpCodes.Stloc, val);
                            il.Emit(OpCodes.Newobj, _valueWrapperCtor);
                            il.Emit(OpCodes.Dup);
                            il.Emit(OpCodes.Ldloc, val);
                            il.Emit(OpCodes.Stfld, _valueWrapperField);
                        }
                        il.Emit(OpCodes.Stloc, result);
                        if (!Method.IsStatic) il.Emit(OpCodes.Ldarg_0);
                        il.Emit(OpCodes.Ldloc, result);
                        il.Emit(OpCodes.Volatile);
                        il.Emit(Method.IsStatic ? OpCodes.Stsfld : OpCodes.Stfld, _valueFieldReference);
                    }
                });
            }
            il.Emit(OpCodes.Ldloc, result);
            if (_valueWrapper != null)
            {
                il.Emit(OpCodes.Ldfld, _valueWrapperField);
            }
            il.Emit(OpCodes.Ret);
        }
    }
}
