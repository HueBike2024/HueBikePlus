using Core.Extensions;

namespace Core.Helpers
{
    public static class EnumHelpers
    {
        public static List<EnumModel> ToEnumLists<T>()
        {
            var enumModels = new List<EnumModel>();
            var t = typeof(T);
            if (!t.IsEnum) return enumModels;

            var values = Enum.GetValues(t).Cast<Enum>().Where(x => !x.GetIgnore()).Select(e => new
            {
                Value = e.GetHashCode(),
                Name = e.GetDescription()
            });

            enumModels.AddRange(values.Select(item => new EnumModel { Value = item.Value, Name = item.Name }));
            return enumModels;
        }

        public static List<EnumModel> ToEnumLists<T>(List<int> elements)
        {
            var enumModels = new List<EnumModel>();
            var t = typeof(T);
            if (!t.IsEnum) return enumModels;

            var values = Enum.GetValues(t).Cast<Enum>().Where(x => !x.GetIgnore() && !elements.Contains(x.GetHashCode())).Select(e => new
            {
                Value = e.GetHashCode(),
                Name = e.GetDescription()
            });

            enumModels.AddRange(values.Select(item => new EnumModel { Value = item.Value, Name = item.Name }));
            return enumModels;
        }

        public static List<EnumModel> ToEnumDescriptionPlusLists<T>()
        {
            var enumModels = new List<EnumModel>();
            var t = typeof(T);
            if (!t.IsEnum) return enumModels;

            var values = Enum.GetValues(t).Cast<Enum>()
                .Where(x => !x.GetIgnore() && !string.IsNullOrEmpty(x.GetDescriptionPlus()))
                .Select(e => new
                {
                    Value = e.GetHashCode(),
                    Name = e.GetDescriptionPlus()
                });

            enumModels.AddRange(values.Select(item => new EnumModel { Value = item.Value, Name = item.Name }));
            return enumModels;
        }
    }

    public class EnumModel
    {
        /// <summary>
        /// Code
        /// </summary>
        public int? Value { get; set; }

        /// <summary>
        /// Name enum
        /// </summary>
        public string Name { get; set; }
    }
}