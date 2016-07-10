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
            ObjRef = method.Module.Import(typeof(object));
            ObjCtorRef = method.Module.Import(new MethodReference(".ctor", method.Module.TypeSystem.Void, ObjRef)
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
