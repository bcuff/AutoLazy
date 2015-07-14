using System;
using System.Linq;

namespace AutoLazy.TestAssembly
{
    public static class StaticClassWithStaticConstructor
    {
        static StaticClassWithStaticConstructor()
        {
            Console.Write("foo");
        }

        [Lazy]
        public static string GetFoo()
        {
            return "a" + 1;
        }

        public static string Foo
        {
            [Lazy]
            get
            {
                return "c" + 3;
            }
        }
    }
}
