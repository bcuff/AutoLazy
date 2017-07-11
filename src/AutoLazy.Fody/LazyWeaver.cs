using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mono.Cecil;
using Mono.Cecil.Cil;
using Mono.Cecil.Rocks;

namespace AutoLazy.Fody
{
    internal abstract class LazyWeaver
    {
        protected readonly MethodDefinition Method;
        protected readonly VisitorContext Context;
        protected readonly TypeReference ObjRef;
        protected readonly MethodReference ObjCtorRef;
        protected FieldDefinition SyncRootField;
        protected FieldReference SyncRootFieldRef;

        protected LazyWeaver(MethodDefinition method, VisitorContext context)
        {
            Method = method;
            Context = context;
            ObjRef = method.Module. ImportReference ( typeof(object));
            ObjCtorRef = method.Module. ImportReference ( new MethodReference(".ctor", method.Module.TypeSystem.Void, ObjRef)
            {
                HasThis = true,
            });
        }

        protected virtual bool Validate()
        {
            var valid = true;
            if (Method.ReturnType.MetadataType == MetadataType.Void)
            {
                Context.LogError("[Lazy] methods must have a non-void return type.", Method);
                valid = false;
            }
            if (Method.HasGenericParameters)
            {
                Context.LogError("[Lazy] is not supported on generic methods.", Method);
                valid = false;
            }
            var bannedPropertyMethods =
                from prop in Method.DeclaringType.Properties
                where prop.SetMethod != null
                select prop.GetMethod;
            if (bannedPropertyMethods.Contains(Method))
            {
                Context.LogError("[Lazy] properties may not have a setter.", Method);
            }
            return valid;

        }

        protected virtual void InitializeTypes() { }

        protected virtual void CreateFields()
        {
            var fieldAttributes = FieldAttributes.Private;
            if (Method.IsStatic) fieldAttributes |= FieldAttributes.Static;
            SyncRootField = new FieldDefinition(Method.Name + "$SyncRoot", fieldAttributes | FieldAttributes.InitOnly, ObjRef);
            Method.DeclaringType.Fields.Add(SyncRootField);
            if (Method.DeclaringType.HasGenericParameters)
            {
                var declaringType = Method.DeclaringType.MakeGenericInstanceType(
                    Method.DeclaringType.GenericParameters.Cast<TypeReference>().ToArray());
                SyncRootFieldRef = new FieldReference(SyncRootField.Name, ObjRef, declaringType);
            }
            else
            {
                SyncRootFieldRef = SyncRootField;
            }
        }

        protected void InitializeFields()
        {
            if (Method.IsStatic)
            {
                var ctor = Method.DeclaringType.GetOrCreateStaticConstructor();
                InitializeFields(ctor);
            }
            else
            {
                foreach (var ctor in Method.DeclaringType.GetConstructors().Where(c => !c.IsStatic))
                {
                    InitializeFields(ctor);
                }
            }
        }

        protected virtual void InitializeFields(MethodDefinition ctor)
        {
            var il = ctor.Body.GetILProcessor();
            var start = ctor.Body.Instructions.First();
            if (!Method.IsStatic) il.InsertBefore(start, il.Create(OpCodes.Ldarg_0));
            il.InsertBefore(start, il.Create(OpCodes.Newobj, ObjCtorRef));
            il.InsertBefore(start, il.Create(Method.IsStatic ? OpCodes.Stsfld : OpCodes.Stfld, SyncRootFieldRef));
        }

        protected void PrepForRewrite()
        {
            if (Method.Body.Instructions.Count(i => i.OpCode == OpCodes.Ret) == 1)
            {
                var instr = Method.Body.Instructions.Single(i => i.OpCode == OpCodes.Ret);
                instr.OpCode = OpCodes.Nop;
                return;
            }

            var result = new VariableDefinition(Method.ReturnType);
            Method.Body.Variables.Add(result);
            var il = Method.Body.GetILProcessor();
            var last = il.Create(OpCodes.Ldloc, result);
            il.Append(last);

            var returns = Method.Body.Instructions.Where(i => i.OpCode == OpCodes.Ret).ToList();
            foreach (var ret in returns)
            {
                il.InsertBefore(ret, il.Create(OpCodes.Stloc, result));
                if (ret.Next == last)
                {
                    ret.OpCode = OpCodes.Nop;
                }
                else
                {
                    ret.OpCode = OpCodes.Br;
                    ret.Operand = last;
                }
            }
        }

        protected virtual void WriteInstructions() { }

        public bool Instrument()
        {
            if (Validate())
            {
                InitializeTypes();
                CreateFields();
                InitializeFields();
                WriteInstructions();
                return true;
            }
            return false;
        }
    }
}
