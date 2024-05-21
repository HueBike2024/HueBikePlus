using System;
using System.Collections.Generic;

namespace Core.Helpers
{
    public static class EnumerableHelper
    {
        /// <summary>
        /// Splitting a list or collection into chunks
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public static IEnumerable<List<T>> Split<T>(List<T> list, int size = 50)
        {
            for (var i = 0; i < list.Count; i += size)
            {
                yield return list.GetRange(i, Math.Min(size, list.Count - i));
            }
        }

        /// <summary>
        /// Add element into Enumerable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumerable"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static IEnumerable<T> Add<T>(this IEnumerable<T> enumerable, T value)
        {
            foreach (var item in enumerable)
                yield return item;

            yield return value;
        }

        /// <summary>
        /// Insert
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumerable"></param>
        /// <param name="index"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static IEnumerable<T> Insert<T>(this IEnumerable<T> enumerable, int index, T value)
        {
            int current = 0;
            foreach (var item in enumerable)
            {
                if (current == index)
                    yield return value;

                yield return item;
                current++;
            }
        }

        /// <summary>
        /// Replace
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumerable"></param>
        /// <param name="index"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static IEnumerable<T> Replace<T>(this IEnumerable<T> enumerable, int index, T value)
        {
            int current = 0;
            foreach (var item in enumerable)
            {
                yield return current == index ? value : item;
                current++;
            }
        }

        /// <summary>
        /// Remove
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumerable"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static IEnumerable<T> Remove<T>(this IEnumerable<T> enumerable, int index)
        {
            int current = 0;
            foreach (var item in enumerable)
            {
                if (current != index)
                    yield return item;

                current++;
            }
        }
    }
}
