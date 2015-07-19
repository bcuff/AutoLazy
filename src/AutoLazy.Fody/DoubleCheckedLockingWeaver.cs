using System;
using System.Linq;
using System.Reflection;
using AutoLazy.Fody;
using Mono.Cecil;
using Mono.Cecil.Cil;
using Mono.Cecil.Rocks;
using FieldAttributes = Mono.Cecil.FieldAttributes;
using MethodAttributes = Mono.Cecil.MethodAttributes;

namespace AutoLazy.Fody
{
    internal static class DoubleCheckedLockingWeaver
    {
        public static void Instrument(MethodDefinition method)
        {
            var implMethod = CopyToPrivateMethod(method, method.Name + "$Impl");
            method.Body.Variables.Clear();
            method.Body.Instructions.Clear();
            method.Body.ExceptionHandlers.Clear();

            var fieldAttributes = FieldAttributes.Private;
            if (method.IsStatic) fieldAttributes |= FieldAttributes.Static;
            var valueField = new FieldDefinition(method.Name + "$Value", fieldAttributes, method.ReturnType);
            method.DeclaringType.Fields.Add(valueField);

            var objRef = method.Module.Import(typeof(object));
            var syncRootField = new FieldDefinition(method.Name + "$SyncRoot", fieldAttributes | FieldAttributes.InitOnly, objRef);
            method.DeclaringType.Fields.Add(syncRootField);
            var objCtorRef = method.DeclaringType.Module.Import(typeof(object).GetConstructors(BindingFlags.CreateInstance | BindingFlags.Instance | BindingFlags.Public).Single());

            method.DeclaringType.GetOrCreateStaticConstructor();
            foreach (var ctor in method.DeclaringType.GetConstructors().Where(c => c.IsStatic == method.IsStatic))
            {
                var il = ctor.Body.GetILProcessor();
                var start = ctor.Body.Instructions.First();
                if (!method.IsStatic) il.InsertBefore(start, il.Create(OpCodes.Ldarg_0));
                il.InsertBefore(start, il.Create(OpCodes.Newobj, objCtorRef));
                il.InsertBefore(start, il.CreateStore(syncRootField));
            }

            WriteInstructions(method, valueField, implMethod, syncRootField);
            method.Body.OptimizeMacros();
        }

        private static void WriteInstructions(MethodDefinition method, FieldDefinition valueField, MethodDefinition methodImpl, FieldDefinition syncRootField)
        {
            var il = method.Body.GetILProcessor();
            var result = new VariableDefinition(method.ReturnType);
            method.Body.InitLocals = true;
            method.Body.Variables.Add(result);
            if (!method.IsStatic) il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Volatile);
            il.EmitLoad(valueField);
            il.Emit(OpCodes.Stloc, result);
            il.Emit(OpCodes.Ldloc, result);
            using (il.BranchIfTrue())
            {
                il.EmitLock(() =>
                {
                    if (!method.IsStatic) il.Emit(OpCodes.Ldarg_0);
                    il.EmitLoad(syncRootField);
                }, () =>
                {
                    if (!method.IsStatic) il.Emit(OpCodes.Ldarg_0);
                    il.EmitLoad(valueField);
                    il.Emit(OpCodes.Stloc, result);
                    il.Emit(OpCodes.Ldloc, result);
                    using (il.BranchIfTrue())
                    {
                        if (!method.IsStatic)
                        {
                            // once for the call & once for the store field
                            il.Emit(OpCodes.Ldarg_0);
                            il.Emit(OpCodes.Ldarg_0);
                        }
                        il.EmitCall(methodImpl);
                        il.Emit(OpCodes.Stloc, result);
                        il.Emit(OpCodes.Ldloc, result);
                        il.Emit(OpCodes.Volatile);
                        il.EmitStore(valueField);
                    }
                });
            }
            il.Emit(OpCodes.Ldloc, result);
            il.Emit(OpCodes.Ret);
            method.Body.OptimizeMacros();
        }

        private static MethodDefinition CopyToPrivateMethod(MethodDefinition method, string name)
        {
            var methodAttributes = method.Attributes;
            methodAttributes |= MethodAttributes.Private;
            methodAttributes &= ~MethodAttributes.Public;
            methodAttributes &= ~MethodAttributes.FamANDAssem;
            methodAttributes &= ~MethodAttributes.Family;
            var implMethod = new MethodDefinition(name, methodAttributes, method.ReturnType)
            {
                Body = { InitLocals = true }
            };
            foreach (var instruction in method.Body.Instructions)
            {
                implMethod.Body.Instructions.Add(instruction);
            }
            foreach (var local in method.Body.Variables)
            {
                implMethod.Body.Variables.Add(local);
            }
            foreach (var handler in method.Body.ExceptionHandlers)
            {
                implMethod.Body.ExceptionHandlers.Add(handler);
            }
            method.DeclaringType.Methods.Add(implMethod);
            return implMethod;
        }
    }
}
