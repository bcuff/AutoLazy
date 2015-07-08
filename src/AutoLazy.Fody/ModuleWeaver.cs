using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Rocks;

namespace AutoLazy.Fody
{
    public class ModuleWeaver
    {
        public ModuleDefinition ModuleDefinition { get; set; }

        public void Execute()
        {
            foreach (var method in GetMethods().ToList())
            {
                Instrument(method);
            }
        }

        private static void Instrument(MethodDefinition method)
        {
            DoubleCheckedLockingWeaver.Instrument(method);
        }

        private IEnumerable<MethodDefinition> GetMethods()
        {
            return from type in ModuleDefinition.Types
                   from method in type.Methods
                   let attribute = GetLazyAttribute(method)
                   where attribute != null
                   select method;
        }

        private static CustomAttribute GetLazyAttribute(ICustomAttributeProvider method)
        {
            return method.CustomAttributes.FirstOrDefault(attr => attr.AttributeType.FullName == "AutoLazy.LazyAttribute");
        }
    }
}
