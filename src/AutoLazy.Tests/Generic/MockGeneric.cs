using System;

namespace AutoLazy.Tests.Generic
{
    public class MockGeneric<T>
    {
        readonly Func<T> _factory;

        public MockGeneric(Func<T> factory)
        {
            _factory = factory;
        }

        [Lazy]
        public T GetValue()
        {
            GetValueCount++;
            return _factory();
        }

        public int GetValueCount { get; private set; }
    }
}
