using System;

namespace AutoLazy.Tests.Generic
{
    public class MockGenericKeyed<T>
    {
        readonly Func<T> _factory;

        public MockGenericKeyed(Func<T> factory)
        {
            _factory = factory;
        }

        [Lazy]
        public T GetValue(int param)
        {
            GetValueCount++;
            return _factory();
        }

        public int GetValueCount { get; private set; }
    }

    /*public class MockGenericKeyedDesired<T>
    {
        readonly Func<T> _factory;
        readonly ConcurrentDictionary<int, T> _cache = new ConcurrentDictionary<int, T>();

        public MockGenericKeyedDesired(Func<T> factory)
        {
            _factory = factory;
        }

        public T GetValue(int param)
        {
            return _cache.GetOrAdd(param, GetValue_Internal);
        }

        public T GetValue_Internal(int param)
        {
            GetValueCount++;
            return _factory();
        }

        public int GetValueCount { get; private set; }
    }

    public static class MockGenericKeyedStaticDesired<T>
    {
        static readonly ConcurrentDictionary<int, T> _cache = new ConcurrentDictionary<int, T>();
        
        public static T GetValue(int param)
        {
            return _cache.GetOrAdd(param, GetValue_Internal);
        }

        public static T GetValue_Internal(int param)
        {
            return default(T);
        }
    }*/
}
