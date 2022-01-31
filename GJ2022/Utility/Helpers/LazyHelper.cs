using System.Collections.Generic;

namespace GJ2022.Utility.Helpers
{
    public static class LazyHelper
    {

        public static void LazyAssocAdd<K, V>(Dictionary<K, List<V>> l, K key, V value)
        {
            if (l.ContainsKey(key))
                l[key].Add(value);
            else
                l.Add(key, new List<V>() { value });
        }

        public static void LazyIntegerAdd<K>(Dictionary<K, int> l, K key, int value)
        {
            if (l.ContainsKey(key))
                l[key] += value;
            else
                l.Add(key, value);
        }

    }
}
