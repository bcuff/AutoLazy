using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Remoting.Channels;
using Mono.Cecil;
using Mono.Cecil.Cil;
using Mono.Cecil.Rocks;

namespace AutoLazy.Fody
{
    public class ModuleWeaver
    {
        public ModuleDefinition ModuleDefinition { get; set; }

        public Action<string> LogError { get; set; }

        public Action<string, SequencePoint> LogErrorPoint { get; set; }

        public void Execute()
        {
            var context = new VisitorContext(this);
            VisitProperties(context);
            VisitMethods(context);
        }

        private void VisitProperties(VisitorContext context)
        {
            var propertyVisitors = GetImplementations<IPropertyVisitor>();
            var q = from type in ModuleDefinition.Types
                    from property in type.Properties
                    select property;
            foreach (var property in q.ToList())
            {
                foreach (var visitor in propertyVisitors)
                {
                    visitor.Visit(property, context);
                }
            }
        }

        private void VisitMethods(VisitorContext context)
        {
            var methodVisitors = GetImplementations<IMethodVisitor>();
            var q = from type in ModuleDefinition.Types
                    from method in type.GetMethods()
                    select method;
            foreach (var method in q.ToList())
            {
                foreach (var visitor in methodVisitors)
                {
                    visitor.Visit(method, context);
                }
            }
        }

        private T[] GetImplementations<T>()
        {
            var visitors = from type in GetType().Assembly.GetTypes()
                           where !type.IsAbstract
                           where !type.IsInterface
                           where typeof(T).IsAssignableFrom(type)
                           select (T)Activator.CreateInstance(type);
            return visitors.ToArray();
        }
    }
}
