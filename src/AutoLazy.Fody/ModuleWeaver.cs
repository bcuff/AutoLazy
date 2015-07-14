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
            foreach (var method in GetMethods().ToList())
            {
                Instrument(method);
            }
        }

        private void LogMethodError(string message, MethodDefinition method)
        {
            var sequencePoint = method.Body.Instructions.Select(i => i.SequencePoint).FirstOrDefault();
            if (sequencePoint == null)
            {
                LogError(string.Format("{0} - see {1}.{2}", message, method.DeclaringType.FullName, method.Name));
            }
            else
            {
                LogErrorPoint(message, sequencePoint);
            }
        }

        private void Instrument(MethodDefinition method)
        {
            if (IsValid(method))
            {
                DoubleCheckedLockingWeaver.Instrument(method);
            }
        }

        private bool IsValid(MethodDefinition method)
        {
            var valid = true;
            if (method.Parameters.Count > 0)
            {
                LogMethodError("[Lazy] methods may not have any parameters.", method);
                valid = false;
            }
            if (method.ReturnType.MetadataType == MetadataType.Void)
            {
                LogMethodError("[Lazy] methods must have a non-void return type.", method);
                valid = false;
            }
            var bannedPropertyMethods =
                from prop in method.DeclaringType.Properties
                where prop.SetMethod != null
                select prop.GetMethod;
            if (bannedPropertyMethods.Contains(method))
            {
                LogMethodError("[Lazy] properties may not have a setter.", method);
            }
            return valid;
        }

        private IEnumerable<MethodDefinition> GetMethods()
        {
            return from type in ModuleDefinition.Types
                   let properties = from prop in type.Properties
                                    where GetLazyAttribute(prop) != null
                                    select prop.GetMethod
                   let methods = from method in type.Methods
                                 where GetLazyAttribute(method) != null
                                 select method
                   from method in properties.Concat(methods).Distinct()
                   select method;
        }

        private static CustomAttribute GetLazyAttribute(ICustomAttributeProvider method)
        {
            return method.CustomAttributes.FirstOrDefault(attr => attr.AttributeType.FullName == "AutoLazy.LazyAttribute");
        }
    }
}
