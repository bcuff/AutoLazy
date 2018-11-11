﻿using Mono.Cecil;

namespace AutoLazy.Fody
{
    internal interface IMethodVisitor
    {
        void Visit(MethodDefinition method, VisitorContext context);
    }
}
