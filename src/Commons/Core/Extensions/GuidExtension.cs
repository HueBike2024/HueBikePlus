using System;
using System.Linq;

namespace Core.Extensions
{
    public static class GuidExtension
    {
        public static bool IsValid(this Guid? val)
        {
            return val != Guid.Empty && val.HasValue;
        }

        public static bool IsValid(this Guid val)
        {
            return val != Guid.Empty;
        }

        public static string ToStringIds(this Guid[] ids)
        {
            return ids?.Any() == true ? string.Join(",", ids) : string.Empty;
        }

        public static string ToStringIds(this string[] ids)
        {
            return ids?.Any() == true ? string.Join(",", ids) : string.Empty;
        }

        public static bool IsGuid(this string value)
        {
            Guid x;
            return Guid.TryParse(value, out x);
        }
    }
}
