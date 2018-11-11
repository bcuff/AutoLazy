using System;

namespace AutoLazy.Tests.Generic
{
    public static class MockGenericStatic<T>
    {
        public static Func<T> Factory;

        [Lazy]
        public static T GetValue()
        {
            GetValueCount++;
            return Factory();
        }

        public static int GetValueCount { get; private set; }
    }
}
