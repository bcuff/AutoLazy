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

        public string Foo
        {
            [Lazy]
            get
            {
                return "c" + 3;
            }
        }

        [Lazy]
        public string Bar
        {
            get
            {
                return "d" + 4;
            }
        }

        [Lazy]
        public Guid Car
        {
            get { return Guid.NewGuid(); }
        }
    }
}
