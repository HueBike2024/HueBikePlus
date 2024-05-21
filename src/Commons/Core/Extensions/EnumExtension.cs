using Core.Attributes;
using System;
using System.Globalization;

namespace Core.Extensions
{
    public static class EnumExtension
    {
        public static string GetDescription<T>(this T e) where T : IConvertible
        {
            string description = null;

            if (!(e is Enum))
            {
                return null;
            }

            var type = e.GetType();
            var values = Enum.GetValues(type);

            foreach (int val in values)
            {
                if (val != e.ToInt32(CultureInfo.InvariantCulture)) continue;
                var memInfo = type.GetMember(type.GetEnumName(val));
                var descriptionAttributes = memInfo[0].GetCustomAttributes(typeof(LocalizationAttribute), false);
                if (descriptionAttributes.Length > 0)
                {
                    description = ((LocalizationAttribute)descriptionAttributes[0]).Description;
                }

                break;
            }

            return description;
        }

        public static string GetDescriptionPlus<T>(this T e) where T : IConvertible
        {
            string description = null;

            if (!(e is Enum))
            {
                return null;
            }

            var type = e.GetType();
            var values = Enum.GetValues(type);

            foreach (int val in values)
            {
                if (val != e.ToInt32(CultureInfo.InvariantCulture)) continue;
                var memInfo = type.GetMember(type.GetEnumName(val));
                var descriptionAttributes = memInfo[0].GetCustomAttributes(typeof(LocalizationAttributePlus), false);
                if (descriptionAttributes.Length > 0)
                {
                    description = ((LocalizationAttributePlus)descriptionAttributes[0]).Description;
                }

                break;
            }

            return description;
        }

        public static bool GetIgnore<T>(this T e) where T : IConvertible
        {
            var isIgnore = false;

            if (!(e is Enum))
            {
                return false;
            }

            var type = e.GetType();
            var values = Enum.GetValues(type);

            foreach (int val in values)
            {
                if (val != e.ToInt32(CultureInfo.InvariantCulture)) continue;
                var memInfo = type.GetMember(type.GetEnumName(val));
                var descriptionAttributes = memInfo[0].GetCustomAttributes(typeof(LocalizationAttribute), false);
                if (descriptionAttributes.Length > 0)
                {
                    isIgnore = ((LocalizationAttribute)descriptionAttributes[0]).IsIgnore;
                }

                break;
            }

            return isIgnore;
        }
    }
}