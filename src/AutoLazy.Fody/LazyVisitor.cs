﻿using System.Linq;
using Mono.Cecil;

namespace AutoLazy.Fody
{
    internal class LazyVisitor : IMethodVisitor, IPropertyVisitor
    {
        public void Visit(MethodDefinition method, VisitorContext context)
        {
            var lazyAttribute = GetLazyAttribute(method);
            if (lazyAttribute != null)
            {
                var instrumentor = GetWeaver(method, context);
                instrumentor.Instrument();
                method.CustomAttributes.Remove(lazyAttribute);
            }
        }

        public void Visit(PropertyDefinition property, VisitorContext context)
        {
            var lazyAttribute = GetLazyAttribute(property);
            if (lazyAttribute != null && property.GetMethod != null && !IsLazy(property.GetMethod))
            {
                var instrumentor = GetWeaver(property.GetMethod, context);
                instrumentor.Instrument();
                property.CustomAttributes.Remove(lazyAttribute);
            }
        }

        private bool IsLazy(ICustomAttributeProvider target)
        {
            return target.HasCustomAttributes && target.CustomAttributes.Any(a => a.AttributeType.FullName == "AutoLazy.LazyAttribute");
        }

        private CustomAttribute GetLazyAttribute(ICustomAttributeProvider target)
        {
            return target.HasCustomAttributes ? target.CustomAttributes.FirstOrDefault(a => a.AttributeType.FullName == "AutoLazy.LazyAttribute") : null;
        }

        private LazyWeaver GetWeaver(MethodDefinition method, VisitorContext context)
        {
            return method.Parameters.Count == 0
                ? (LazyWeaver) new DoubleCheckedLockingWeaver(method, context)
                : new KeyedLazyWeaver(method, context);
        }
    }
}
