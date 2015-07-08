using System;
using System.Linq;

namespace AutoLazy.TestAssembly
{
    public static class StaticClassWithoutStaticConstructor
    {
        [Lazy]
        public static string GetFoo()
        {
            return "a" + 1;
        }
    }
}
