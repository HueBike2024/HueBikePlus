using System;
using System.Text.RegularExpressions;

namespace Core.Helpers
{
    public static class NumberHelper
    {
        public static bool CheckNumber(string text)
        {
            Regex regex = new Regex(@"^[-+]?[0-9]*.?[0-9]+$");
            return !regex.IsMatch(text);
        }

        /// <summary>
        /// Kiểm tra số decimal (18,3) dương
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static bool CheckDecimalPositiveMax(string text)
        {
            Regex regex = new Regex(@"^[0-9]{1,15}(?:\.[0-9]{1,3})?$");
            return regex.IsMatch(text);
        }

        /// <summary>
        /// Kiểm tra số decimal (18,3) dương
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static bool IsDecimal(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return false;

            return Decimal.TryParse(text, out decimal value);
        }
    }
}
