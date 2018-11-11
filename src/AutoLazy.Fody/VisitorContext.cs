﻿using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace AutoLazy.Fody
{
    internal class VisitorContext
    {
        private readonly ModuleWeaver _weaver;

        public VisitorContext(ModuleWeaver weaver)
        {
            _weaver = weaver;
        }

        public void LogInfo(string message)
        {
            _weaver.LogInfo(message);
        }

        public void LogError(string message, MethodDefinition method)
        {
            var point = (method.DebugInformation?.HasSequencePoints ?? false)
                ? method.DebugInformation.SequencePoints.FirstOrDefault()
                : null;
            if (point == null)
            {
                message = $"{message} - in method {method.DeclaringType.FullName}.{method.Name}.";
            }
            LogError(message, point);
        }

        public void LogError(string message, SequencePoint point = null)
        {
            if (point == null)
            {
                _weaver.LogError(message);
            }
            else
            {
                _weaver.LogErrorPoint(message, point);
            }
        }
    }
}
