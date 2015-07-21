using System;
using System.Linq;
using Mono.Cecil;

namespace AutoLazy.Fody
{
    internal class LazyVisitor : IMethodVisitor, IPropertyVisitor
    {
        public void Visit(MethodDefinition method, VisitorContext context)
        {
            if (IsLazy(method))
            {
                var instrumentor = new DoubleCheckedLockingWeaver(method, context);
                instrumentor.Instrument();
            }
        }

        public void Visit(PropertyDefinition property, VisitorContext context)
        {
            if (IsLazy(property) && property.GetMethod != null && !IsLazy(property.GetMethod))
            {
                var instrumentor = new DoubleCheckedLockingWeaver(property.GetMethod, context);
                instrumentor.Instrument();
            }
        }

        private bool IsLazy(ICustomAttributeProvider target)
        {
            return target.HasCustomAttributes && target.CustomAttributes.Any(a => a.AttributeType.FullName == "AutoLazy.LazyAttribute");
        }
    }
}
