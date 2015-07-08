using System;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;
using Mono.Cecil.Rocks;

namespace AutoLazy.Fody
{
    internal static class TypeExtensions
    {
        const MethodAttributes _staticConstructorAttributes
            = MethodAttributes.Private
            | MethodAttributes.HideBySig
            | MethodAttributes.SpecialName
            | MethodAttributes.RTSpecialName
            | MethodAttributes.Static;

        public static MethodDefinition GetOrCreateStaticConstructor(this TypeDefinition type)
        {
            var cctor = type.GetStaticConstructor();
            if (cctor != null) return cctor;
            cctor = new MethodDefinition(".cctor", _staticConstructorAttributes, type.Module.Import(typeof(void)));
            type.Methods.Add(cctor);
            var il = cctor.Body.GetILProcessor();
            il.Emit(OpCodes.Ret);
            return cctor;
        }
    }
}
