using System;
using System.Linq;

namespace AutoLazy.TestAssembly
{
    public class GenericInstanceClassExample<T>
    {
        readonly object _syncRoot = new object();
        FooWrapper _value;

        public T GetFoo()
        {
            var result = _value;
            if (result == null)
            {
                lock (_syncRoot)
                {
                    result = _value;
                    if (result == null)
                    {
                        result = new FooWrapper
                        {
                            Value = default(T)
                        };
                        _value = result;
                    }
                }
            }
            return result.Value;
        }

        private sealed class FooWrapper
        {
            public T Value;
        }
    }
}
