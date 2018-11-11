using System;

namespace AutoLazy.Tests.NonGeneric
{
    public static class MockNonGenericStatic
    {
        public static Func<int> Factory;

        [Lazy]
        public static int GetValue()
        {
            GetValueCount++;
            return Factory();
        }

        public static int GetValueCount { get; private set; }
    }
}
