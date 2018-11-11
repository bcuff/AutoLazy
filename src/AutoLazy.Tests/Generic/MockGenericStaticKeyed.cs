using System;

namespace AutoLazy.Tests.Generic
{
    public static class MockGenericStaticKeyed<T>
    {
        public static Func<T, T> Factory;

        [Lazy]
        public static T GetValue(T param)
        {
            GetValueCount++;
            return Factory(param);
        }

        public static int GetValueCount { get; private set; }
    }
}
