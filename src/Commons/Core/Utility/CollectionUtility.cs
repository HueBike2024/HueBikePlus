using System;
using System.Collections.Generic;
using System.Linq;

namespace Core.Utility
{
    public static class CollectionUtility
    {
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> list)
        {
            return !(list?.Any(t => t != null)).GetValueOrDefault();
        }

        public static void AddRange<TKey, TValue>(this Dictionary<TKey, TValue> dic, List<KeyValuePair<TKey, TValue>> itemsToAdd)
        {
            itemsToAdd.ForEach(x => dic.Add(x.Key, x.Value));
        }

        public static string DicToString<TKey, TValue>(this IDictionary<TKey, TValue> dictionary)
        {
            if (dictionary == null)
                throw new ArgumentNullException("dictionary");

            var items = from kvp in dictionary
                        select kvp.Key + "=" + kvp.Value;

            return "{" + string.Join(",", items) + "}";
        }
    }
}
