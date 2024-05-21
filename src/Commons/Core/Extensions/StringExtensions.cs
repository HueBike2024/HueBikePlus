using Core.Common;
using Newtonsoft.Json;
using System;
using System.Text;
using System.Text.RegularExpressions;

namespace Core.Extensions
{
    public static class StringExtensions
    {
        public static string RemoveSignedVietnameseString(this string str)
        {
            if (string.IsNullOrEmpty(str)) return string.Empty;
            Regex regex = new Regex("\\p{IsCombiningDiacriticalMarks}+");
            string temp = str.Normalize(NormalizationForm.FormD);
            return (regex.Replace(temp, String.Empty).Replace('\u0111', 'd').Replace('\u0110', 'D').ToLower()).Trim();
        }

        /// <summary>
		/// Convert a string to camel case
		/// </summary>
		/// <param name="str"></param>
		/// <returns></returns>
		public static string ToCamelCase(this string str)
        {
            if (!string.IsNullOrEmpty(str) && str.Length > 1)
            {
                return char.ToLowerInvariant(str[0]) + str.Substring(1);
            }
            return str;
        }

        public static string ToIsoString(this DateTime? dateTime)
        {
            if (dateTime == null) return null;
            return dateTime.Value.ToString(AppConstants.DateTimeFormat.DateTimeIsoString);
        }

        public static string ToJson(this object obj)
        {
            if (obj == null) return string.Empty;
            return JsonConvert.SerializeObject(obj);
        }

        public static string ToIsoString(this DateTime dateTime)
        {
            return dateTime.ToString(AppConstants.DateTimeFormat.DateTimeIsoString);
        }

        public static bool IsValid(this string val)
        {
            return !string.IsNullOrWhiteSpace(val);
        }

        public static string ToDateString(this DateTime? input)
        {
            if (!input.HasValue) return string.Empty;
            return input.Value.ToString(AppConstants.DateTimeFormat.DateString);
        }

        public static string ToTimeString(this DateTime? input)
        {
            if (!input.HasValue) return string.Empty;
            return input.Value.ToString(AppConstants.DateTimeFormat.TimeString);
        }

        public static bool IsGuid(string value)
        {
            Guid x;
            return Guid.TryParse(value, out x);
        }

        /// <summary>
        /// Truncates string so that it is no longer than the specified number of characters.
        /// </summary>
        /// <param name="str">String to truncate.</param>
        /// <param name="length">Maximum string length.</param>
        /// <returns>Original string or a truncated one if the original was too long.</returns>
        public static string Truncate(this string str, int length)
        {
            int maxLength = Math.Min(str.Length, length);
            var substring = str.Substring(0, maxLength);
            return $"{substring}...";
        }
    }
}
