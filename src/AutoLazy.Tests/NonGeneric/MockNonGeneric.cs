using System;

namespace AutoLazy.Tests.NonGeneric
{
    public class MockNonGeneric
    {
        readonly Func<int> _factory;

        public MockNonGeneric(Func<int> factory)
        {
            _factory = factory;
        }

        [Lazy]
        public int GetValue()
        {
            GetValueCount++;
            return _factory();
        }

        public int GetValueCount { get; private set; }
    }
}
