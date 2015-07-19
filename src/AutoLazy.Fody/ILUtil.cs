using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using Mono.Cecil;
using Mono.Cecil.Cil;
using MethodAttributes = Mono.Cecil.MethodAttributes;

namespace AutoLazy.Fody
{
    internal static class ILUtil
    {
        private static readonly MethodBase _monitorEnter;
        private static readonly MethodBase _monitorExit;

        static ILUtil()
        {
            var monitorMethods = typeof(Monitor).GetMethods(BindingFlags.Static | BindingFlags.Public);
            _monitorEnter = monitorMethods.Single(m =>
            {
                if (m.Name != "Enter") return false;
                var parameters = m.GetParameters();
                return parameters.Length == 2
                    && parameters[0].ParameterType == typeof(object)
                    && parameters[1].ParameterType.IsByRef && parameters[1].ParameterType.FullName == "System.Boolean&";
            });
            _monitorExit = monitorMethods.Single(m =>
            {
                if (m.Name != "Exit") return false;
                var parameters = m.GetParameters();
                return parameters.Length == 1 && parameters[0].IsOut == false && parameters[0].ParameterType == typeof(object);
            });
        }

        public static MethodDefinition CopyToPrivateMethod(this MethodDefinition method, string name)
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

        public static IDisposable BranchIfTrue(this ILProcessor il)
        {
            return il.Branch(OpCodes.Brtrue);
        }

        public static IDisposable BranchIfFalse(this ILProcessor il)
        {
            return il.Branch(OpCodes.Brfalse);
        }

        private static IDisposable Branch(this ILProcessor il, OpCode opCode)
        {
            var target = il.Create(OpCodes.Nop);
            il.Emit(opCode, target);
            return new BranchDisposable(il, target);
        }

        public static void EmitLock(this ILProcessor il, Action loadSyncRoot, Action block)
        {
            var lockTaken = new VariableDefinition("$lockTaken", il.Body.Method.Module.Import(typeof(bool)));
            il.Body.Variables.Add(lockTaken);
            var syncRoot = new VariableDefinition("$root", il.Body.Method.Module.Import(typeof(object)));
            il.Body.Variables.Add(syncRoot);
            il.Emit(OpCodes.Ldc_I4_0);
            il.Emit(OpCodes.Stloc, lockTaken);
            il.EmitTryFinally(() =>
            {
                loadSyncRoot();
                il.Emit(OpCodes.Dup);
                il.Emit(OpCodes.Stloc, syncRoot);
                il.Emit(OpCodes.Ldloca, lockTaken);
                il.Emit(OpCodes.Call, il.Body.Method.Module.Import(_monitorEnter));
                block();
            }, () =>
            {
                il.Emit(OpCodes.Ldloc, lockTaken);
                using (il.BranchIfFalse())
                {
                    il.Emit(OpCodes.Ldloc, syncRoot);
                    il.Emit(OpCodes.Call, il.Body.Method.Module.Import(_monitorExit));
                }
            });
        }

        public static void EmitTryFinally(this ILProcessor il, Action tryBlock, Action finallyBlock)
        {
            var end = il.Create(OpCodes.Nop);
            var middle = il.Create(OpCodes.Nop);
            var handler = new ExceptionHandler(ExceptionHandlerType.Finally)
            {
                TryStart = il.Create(OpCodes.Nop),
                TryEnd = middle,
                HandlerStart = middle,
                HandlerEnd = end,
            };
            il.Append(handler.TryStart);
            tryBlock();
            il.Emit(OpCodes.Leave, end);
            il.Append(middle);
            finallyBlock();
            il.Emit(OpCodes.Endfinally);
            il.Append(end);
            il.Body.ExceptionHandlers.Add(handler);
        }

        public static void EmitMonitorEnter(this ILProcessor il)
        {
            var enter = il.Body.Method.Module.Import(_monitorEnter);
            il.Emit(OpCodes.Call, enter);
        }

        public static Instruction CreateMonitorExit(this ILProcessor il)
        {
            var exit = il.Body.Method.Module.Import(_monitorExit);
            return il.Create(OpCodes.Call, exit);
        }

        public static void EmitMonitorExit(this ILProcessor il)
        {
            il.Append(il.CreateMonitorExit());
        }

        public static void EmitLoad(this ILProcessor il, FieldDefinition field)
        {
            il.Emit(field.IsStatic ? OpCodes.Ldsfld : OpCodes.Ldfld, field);
        }

        public static Instruction CreateStore(this ILProcessor il, FieldDefinition field)
        {
            return il.Create(field.IsStatic ? OpCodes.Stsfld : OpCodes.Stfld, field);
        }

        public static void EmitStore(this ILProcessor il, FieldDefinition field)
        {
            il.Append(il.CreateStore(field));
        }

        public static void EmitCall(this ILProcessor il, MethodDefinition method)
        {
            il.Emit(method.IsVirtual ? OpCodes.Callvirt : OpCodes.Call, method);
        }

        private class BranchDisposable : IDisposable
        {
            readonly ILProcessor _il; 
            readonly Instruction _target;

            public BranchDisposable(ILProcessor il, Instruction target)
            {
                _il = il;
                _target = target;
            }

            public void Dispose()
            {
                _il.Append(_target);
            }
        }
    }
}
