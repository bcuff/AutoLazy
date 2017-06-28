using System;
using System.Linq;
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

        public void LogError(string message, MethodDefinition method)
        {
            //var point = method . Body . Instructions . Select ( i => i . SequencePoint ) . FirstOrDefault ( );
            //if ( point == null )
            //{
                message = string . Format ( "{0} - in method {1}.{2}." , message , method . DeclaringType . FullName , method . Name );
            //}
            LogError ( message /*, point */);
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
