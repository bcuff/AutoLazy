using System;
using System.Linq;

namespace AutoLazy.TestAssembly
{
    public class InstanceClassWithMultipleConstructors
    {
        static InstanceClassWithMultipleConstructors()
        {
            Console.Write("foo");
        }

        public InstanceClassWithMultipleConstructors()
        {
        }

        public InstanceClassWithMultipleConstructors(string foo)
        {
        }

        public InstanceClassWithMultipleConstructors(int foo)
        {
        }

        [Lazy]
        public string GetFoo()
        {
            return "a" + 1;
        }

        [Lazy]
        public string GetBar()
        {
            return "b" + 2;
        }
    }
}
