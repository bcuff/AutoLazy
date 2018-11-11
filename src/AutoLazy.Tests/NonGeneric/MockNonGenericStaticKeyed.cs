using System;

namespace AutoLazy.Tests.NonGeneric
{
    public static class MockNonGenericStaticKeyed
    {
        public static Func<int, int> Factory;

        [Lazy]
        public static int GetValue(int param)
        {
            GetValueCount++;
            return Factory(param);
        }

        public static int GetValueCount { get; private set; }
    }
}