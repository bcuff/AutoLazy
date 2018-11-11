using System;

namespace AutoLazy.TestAssembly
{
    public class ValueTypeExample
    {
        readonly object _syncRoot = new object();
        GuidWrapper _value;

        public Guid GetFoo()
        {
            var result = _value;
            if (result == null)
            {
                lock (_syncRoot)
                {
                    result = _value;
                    if (result == null)
                    {
                        result = new GuidWrapper
                        {
                            Value = GetFooImpl()
                        };
                        _value = result;
                    }
                }
            }
            return result.Value;
        }

        private Guid GetFooImpl()
        {
            return Guid.NewGuid();
        }

        private sealed class GuidWrapper
        {
            public Guid Value;
        }
    }
}
