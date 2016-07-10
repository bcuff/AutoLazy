using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mono.Cecil;
using Mono.Cecil.Cil;
using Mono.Cecil.Rocks;

namespace AutoLazy.Fody
{
    internal class KeyedLazyWeaver : LazyWeaver
    {
        TypeReference _dictionaryRef;
        MethodReference _dictionaryGetEnumerator;
        MethodReference _dictionaryCtorEmpty;
        MethodReference _dictionaryCtorWithCapacity;
        MethodReference _dictionaryAdd;
        MethodReference _dictionaryCount;
        MethodReference _dictionaryTryGetValue;
        FieldDefinition _dictionaryField;
        FieldReference _dictionaryFieldRef;

        TypeReference _disposableRef;
        MethodReference _disposableDispose;

        TypeReference _dictionaryEnumeratorRef;
        MethodReference _dictionaryEnumeratorCurrent;
        MethodReference _dictionaryEnumeratorMoveNext;

        TypeReference _keyValuePairRef;
        MethodReference _keyValuePairKey;
        MethodReference _keyValuePairValue;

        public KeyedLazyWeaver(MethodDefinition method, VisitorContext context)
            : base(method, context)
        {
        }

        protected override bool Validate()
        {
            var valid = true;
            if (Method.Parameters.Count > 1)
            {
                Context.LogError("[Lazy] methods may have at most 1 parameter");
                valid = false;
            }
            return valid && base.Validate();
        }

        protected override void InitializeTypes()
        {
            base.InitializeTypes();
            var genericArgs = new [] { Method.Parameters[0].ParameterType, Method.ReturnType };
            var dict = Method.Module.Import(typeof(Dictionary<,>));
            _dictionaryRef = dict.MakeGenericInstanceType(genericArgs);
            _dictionaryRef.GenericParameters.Add(new GenericParameter(_dictionaryRef));
            _dictionaryRef.GenericParameters.Add(new GenericParameter(_dictionaryRef));
            var ctor = new MethodReference(".ctor", Method.Module.TypeSystem.Void, _dictionaryRef) { HasThis = true };
            _dictionaryCtorEmpty = Method.Module.Import(ctor);
            ctor = new MethodReference(".ctor", Method.Module.TypeSystem.Void, _dictionaryRef)
            {
                HasThis = true,
                Parameters = { new ParameterDefinition(Method.Module.TypeSystem.Int32) }
            };
            _dictionaryCtorWithCapacity = Method.Module.Import(ctor);
            var add = new MethodReference("Add", Method.Module.TypeSystem.Void, _dictionaryRef)
            {
                HasThis = true,
                Parameters =
                {
                    new ParameterDefinition(_dictionaryRef.GenericParameters[0]),
                    new ParameterDefinition(_dictionaryRef.GenericParameters[1]),
                },
            };
            _dictionaryAdd = Method.Module.Import(add);
            var tryGetValue = new MethodReference("TryGetValue", Method.Module.TypeSystem.Boolean, _dictionaryRef)
            {
                HasThis = true,
                Parameters =
                {
                    new ParameterDefinition(_dictionaryRef.GenericParameters[0]),
                    new ParameterDefinition(new ByReferenceType(_dictionaryRef.GenericParameters[1])) { IsOut = true },
                },
            };
            _dictionaryTryGetValue = Method.Module.Import(tryGetValue);
            var count = new MethodReference("get_Count", Method.Module.TypeSystem.Int32, _dictionaryRef) { HasThis = true };
            _dictionaryCount = Method.Module.Import(count);
            var enumerator = Method.Module.Import(typeof(Dictionary<,>.Enumerator));
            var parameterizedEnumerator = enumerator.MakeGenericInstanceType(_dictionaryRef.GenericParameters[0], _dictionaryRef.GenericParameters[1]);
            _dictionaryGetEnumerator = Method.Module.Import(new MethodReference("GetEnumerator", parameterizedEnumerator, _dictionaryRef) { HasThis = true });

            // KeyValuePair
            var kvp = Method.Module.Import(typeof(KeyValuePair<,>));
            _keyValuePairRef = kvp.MakeGenericInstanceType(genericArgs);
            _keyValuePairRef.GenericParameters.Add(new GenericParameter(_keyValuePairRef));
            _keyValuePairRef.GenericParameters.Add(new GenericParameter(_keyValuePairRef));
            _keyValuePairKey = Method.Module.Import(new MethodReference("get_Key", _keyValuePairRef.GenericParameters[0], _keyValuePairRef) { HasThis = true });
            _keyValuePairValue = Method.Module.Import(new MethodReference("get_Value", _keyValuePairRef.GenericParameters[1], _keyValuePairRef) { HasThis = true });

            // Enumerator
            _dictionaryEnumeratorRef = enumerator.MakeGenericInstanceType(genericArgs);
            _dictionaryEnumeratorRef.GenericParameters.Add(new GenericParameter(_dictionaryEnumeratorRef));
            _dictionaryEnumeratorRef.GenericParameters.Add(new GenericParameter(_dictionaryEnumeratorRef));
            var parameterizedKvp = kvp.MakeGenericInstanceType(_dictionaryEnumeratorRef.GenericParameters[0], _dictionaryEnumeratorRef.GenericParameters[1]);
            _dictionaryEnumeratorCurrent = Method.Module.Import(new MethodReference("get_Current", parameterizedKvp, _dictionaryEnumeratorRef) { HasThis = true });
            _dictionaryEnumeratorMoveNext = Method.Module.Import(new MethodReference("MoveNext", Method.Module.TypeSystem.Boolean, _dictionaryEnumeratorRef) { HasThis = true });

            // Dispose
            _disposableRef = Method.Module.Import(typeof(IDisposable));
            _disposableDispose = Method.Module.Import(new MethodReference("Dispose", Method.Module.TypeSystem.Void, _disposableRef) { HasThis = true });
        }

        protected override void CreateFields()
        {
            base.CreateFields();
            var fieldAttributes = FieldAttributes.Private;
            if (Method.IsStatic) fieldAttributes |= FieldAttributes.Static;
            _dictionaryField = new FieldDefinition($"{Method.Name}$LazyCache", fieldAttributes, _dictionaryRef);
            Method.DeclaringType.Fields.Add(_dictionaryField);
            if (Method.DeclaringType.HasGenericParameters)
            {
                var declaringType = Method.DeclaringType.MakeGenericInstanceType(
                    Method.DeclaringType.GenericParameters.Cast<TypeReference>().ToArray());
                _dictionaryFieldRef = new FieldReference(_dictionaryField.Name, _dictionaryField.FieldType, declaringType);
            }
            else
            {
                _dictionaryFieldRef = _dictionaryField;
            }
        }

        protected override void InitializeFields(MethodDefinition ctor)
        {
            var il = ctor.Body.GetILProcessor();
            var start = ctor.Body.Instructions.First();
            if (!Method.IsStatic) il.InsertBefore(start, il.Create(OpCodes.Ldarg_0));
            il.InsertBefore(start, il.Create(OpCodes.Newobj, _dictionaryCtorEmpty));
            il.InsertBefore(start, il.Create(Method.IsStatic ? OpCodes.Stsfld : OpCodes.Stfld, _dictionaryFieldRef));
            base.InitializeFields(ctor);
        }

        protected override void WriteInstructions()
        {
            base.WriteInstructions();
            var bodyInstructions = Method.Body.Instructions.ToList();
            foreach (var instruction in bodyInstructions.Where(i => i.OpCode == OpCodes.Ret))
            {
                instruction.OpCode = OpCodes.Nop;
            }
            Method.Body.Instructions.Clear();
            var il = Method.Body.GetILProcessor();
            var dict = new VariableDefinition(_dictionaryRef);
            var val = new VariableDefinition(Method.ReturnType);
            var e = new VariableDefinition(_dictionaryEnumeratorRef);
            var pair = new VariableDefinition(_keyValuePairRef);
            Method.Body.InitLocals = true;
            Method.Body.Variables.Add(dict);
            Method.Body.Variables.Add(val);
            Method.Body.Variables.Add(e);
            Method.Body.Variables.Add(pair);
            if (!Method.IsStatic) il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Volatile);
            il.Emit(Method.IsStatic ? OpCodes.Ldsfld : OpCodes.Ldfld, _dictionaryFieldRef);
            il.Emit(Method.IsStatic ? OpCodes.Ldarg_0 : OpCodes.Ldarg_1); // load key
            il.Emit(OpCodes.Ldloca, val); // load value address
            il.Emit(OpCodes.Callvirt, _dictionaryTryGetValue);
            using (il.BranchIfTrue())
            {
                il.EmitLock(() =>
                {
                    if (!Method.IsStatic) il.Emit(OpCodes.Ldarg_0);
                    il.Emit(Method.IsStatic ? OpCodes.Ldsfld : OpCodes.Ldfld, SyncRootFieldRef);
                }, () =>
                {
                    if (!Method.IsStatic) il.Emit(OpCodes.Ldarg_0);
                    il.Emit(Method.IsStatic ? OpCodes.Ldsfld : OpCodes.Ldfld, _dictionaryFieldRef);
                    il.Emit(Method.IsStatic ? OpCodes.Ldarg_0 : OpCodes.Ldarg_1); // load key
                    il.Emit(OpCodes.Ldloca, val); // load value address
                    il.Emit(OpCodes.Callvirt, _dictionaryTryGetValue);
                    using (il.BranchIfTrue())
                    {
                        foreach (var instruction in bodyInstructions)
                        {
                            Method.Body.Instructions.Add(instruction);
                        }
                        il.Emit(OpCodes.Stloc, val);

                        // copy dictionary and store to dict variable
                        il.Emit(Method.IsStatic ? OpCodes.Ldsfld : OpCodes.Ldfld, _dictionaryFieldRef);
                        il.Emit(OpCodes.Callvirt, _dictionaryCount);
                        il.Emit(OpCodes.Ldc_I4_1);
                        il.Emit(OpCodes.Add);
                        il.Emit(OpCodes.Newobj, _dictionaryCtorWithCapacity);
                        il.Emit(OpCodes.Stloc, dict);

                        il.Emit(OpCodes.Ldloc, dict);
                        il.Emit(Method.IsStatic ? OpCodes.Ldarg_0 : OpCodes.Ldarg_1); // load key
                        il.Emit(OpCodes.Ldloc, val); // load value
                        il.Emit(OpCodes.Callvirt, _dictionaryAdd);

                        // todo - copy existing vals
                        il.Emit(Method.IsStatic ? OpCodes.Ldsfld : OpCodes.Ldfld, _dictionaryFieldRef);
                        il.Emit(OpCodes.Callvirt, _dictionaryGetEnumerator);
                        il.Emit(OpCodes.Stloc, e);
                        il.EmitTryFinally(() =>
                        {
                            var conditionStart = il.Create(OpCodes.Ldloca, e);
                            il.Emit(OpCodes.Br, conditionStart);

                            var loopStart = il.Create(OpCodes.Ldloca, e);
                            il.Append(loopStart);
                            il.Emit(OpCodes.Call, _dictionaryEnumeratorCurrent);
                            il.Emit(OpCodes.Stloc, pair);

                            il.Emit(OpCodes.Ldloc, dict);
                            il.Emit(OpCodes.Ldloca, pair);
                            il.Emit(OpCodes.Call, _keyValuePairKey);
                            il.Emit(OpCodes.Ldloca, pair);
                            il.Emit(OpCodes.Call, _keyValuePairValue);
                            il.Emit(OpCodes.Callvirt, _dictionaryAdd);

                            // condition part
                            il.Append(conditionStart); // ldloca e
                            il.Emit(OpCodes.Call, _dictionaryEnumeratorMoveNext);
                            il.Emit(OpCodes.Brtrue, loopStart);
                        },
                        () =>
                        {
                            il.Emit(OpCodes.Ldloca, e);
                            il.Emit(OpCodes.Constrained, _dictionaryEnumeratorRef);
                            il.Emit(OpCodes.Callvirt, _disposableDispose);
                        });

                        if (!Method.IsStatic) il.Emit(OpCodes.Ldarg_0);
                        il.Emit(OpCodes.Ldloc, dict);
                        il.Emit(OpCodes.Volatile);
                        il.Emit(Method.IsStatic ? OpCodes.Stsfld : OpCodes.Stfld, _dictionaryFieldRef);
                    }
                });
            }
            il.Emit(OpCodes.Ldloc, val);
            il.Emit(OpCodes.Ret);
        }
    }
}
