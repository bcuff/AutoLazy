using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Remoting.Channels;
using Mono.Cecil;
using Mono.Cecil.Cil;
using Mono.Cecil.Rocks;

using global::Fody;

namespace AutoLazy.Fody
{
    public class ModuleWeaver : BaseModuleWeaver
    {
        public override void Execute()
        {
            var context = new VisitorContext(this);
            VisitProperties(context);
            VisitMethods(context);
            VisitAssemblyReferences(context);
        }

        public override IEnumerable<string> GetAssembliesForScanning()
        {
            yield break;
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
            var q = from type in GetTypes(ModuleDefinition.Types)
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

        private IEnumerable<TypeDefinition> GetTypes(IEnumerable<TypeDefinition> types)
        {
            foreach (var type in types)
            {
                yield return type;
                if (type.NestedTypes != null)
                {
                    foreach (var nested in GetTypes(type.NestedTypes))
                    {
                        yield return nested;
                    }
                }
            }
       }

        private void VisitAssemblyReferences(VisitorContext context)
        {
            // Attempt to locate the "AutoLazy" assembly reference.
            var assemblyName = ModuleDefinition.AssemblyReferences.FirstOrDefault(
                reference => reference.FullName.StartsWith("AutoLazy")
            );

            // Verify our result.
            if(assemblyName == null) {
                // Will happen if AutoLazy is added, but not used.
                return;
            }

            // Remove unnecessary references remaining in our subject assembly.
            ModuleDefinition.AssemblyReferences.Remove(assemblyName);
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
