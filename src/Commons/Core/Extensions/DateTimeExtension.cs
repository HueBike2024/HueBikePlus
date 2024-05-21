using Core.Common;
using System;
using static Core.Common.AppConstants;

namespace Core.Extensions
{
    public static class DateTimeExtension
    {
        public static DateTime ToLocalDateTime(this DateTime date, int timezone)
        {
            return date.AddMinutes(-timezone);
        }

        public static DateTime ToLocalDateTime(this DateTime? date, int timezone)
        {
            return date.Value.AddMinutes(-timezone);
        }

        public static string GetTime(this DateTime dateTime)
        {
            return dateTime.ToString(DateTimeFormat.Hour);
        }

        public static string GetDayOfWeek(this DayOfWeek dayOfWeek)
        {
            var result = string.Empty;
            switch (dayOfWeek)
            {
                case DayOfWeek.Monday:
                    result = "Thứ hai";
                    break;
                case DayOfWeek.Tuesday:
                    result = "Thứ ba";
                    break;
                case DayOfWeek.Wednesday:
                    result = "Thứ tư";
                    break;
                case DayOfWeek.Thursday:
                    result = "Thứ năm";
                    break;
                case DayOfWeek.Friday:
                    result = "Thứ sáu";
                    break;
                case DayOfWeek.Saturday:
                    result = "Thứ bảy";
                    break;
                case DayOfWeek.Sunday:
                    result = "Chủ nhật";
                    break;
            }
            return result;
        }
    }
}
