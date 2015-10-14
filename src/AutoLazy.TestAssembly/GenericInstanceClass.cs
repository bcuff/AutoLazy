using System;
using System.Linq;

namespace AutoLazy.TestAssembly
{
    public class GenericInstanceClass<T>
    {
        [Lazy]
        public T GetFoo()
        {
            return default(T);
        }
    }
}
