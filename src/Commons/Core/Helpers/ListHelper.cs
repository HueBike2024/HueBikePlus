using System;
using System.Collections.Generic;

namespace Core.Helpers
{
    public static class ListHelper
    {
        public static IEnumerable<List<T>> SplitList<T>(List<T> locations, int nSize = 10)
        {
            for (int i = 0; i < locations.Count; i += nSize)
            {
                yield return locations.GetRange(i, Math.Min(nSize, locations.Count - i));
            }
        }
    }
}
