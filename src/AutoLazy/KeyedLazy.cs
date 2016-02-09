using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace AutoLazy
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public class KeyedLazy<TKey, TValue>
    {
        private readonly object _syncRoot = new object();
        private readonly Func<TKey, TValue> _valueFactory;
        private volatile Dictionary<TKey, TValue> _values = new Dictionary<TKey, TValue>();

        public KeyedLazy(Func<TKey, TValue> valueFactory)
        {
            if (valueFactory == null) throw new ArgumentNullException("valueFactory");
            _valueFactory = valueFactory;
        }

        public TValue Get(TKey key)
        {
            TValue result;
            if (_values.TryGetValue(key, out result)) return result;
            lock (_syncRoot)
            {
                var values = _values;
                if (values.TryGetValue(key, out result)) return result;

                var newValue = _valueFactory(key);
                var newDictionary = new Dictionary<TKey, TValue>(values.Count + 1);
                foreach (var pair in values)
                {
                    newDictionary.Add(pair.Key, pair.Value);
                }
                newDictionary.Add(key, newValue);
                _values = newDictionary;
                return newValue;
            }
        }
    }
}
