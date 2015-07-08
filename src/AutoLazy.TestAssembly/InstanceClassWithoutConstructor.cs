using System;
using System.Linq;

namespace AutoLazy.TestAssembly
{
    public class InstanceClassWithoutConstructor
    {
        [Lazy]
        public string GetFoo()
        {
            return "a" + 1;
        }
    }
}
