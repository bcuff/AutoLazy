using System;

namespace AutoLazy.Tests.NonGeneric
{
    public class MockNonGenericKeyed
    {
        readonly Func<int> _factory;

        public MockNonGenericKeyed(Func<int> factory)
        {
            _factory = factory;
        }

        [Lazy]
        public int GetValue(int param)
        {
            GetValueCount++;
            return _factory();
        }

        public int GetValueCount { get; private set; }
    }
}