using System;
using System.Linq;

namespace Core.Helpers
{
    public class DateTimeHelper
    {
        public static DateTime? ConvertToDateFormat(string vnDate)
        {
            string[] dates = vnDate.Split("/");
            if (dates.Any())
            {
                return new DateTime(int.Parse(dates[2]), int.Parse(dates[1]), int.Parse(dates[0]));
            }
            return null;
        }

        public static string DayOfWeekVietNam(DateTime value)
        {
            return value.DayOfWeek switch
            {
                DayOfWeek.Monday => "Thứ hai",
                DayOfWeek.Tuesday => "Thứ ba",
                DayOfWeek.Wednesday => "Thứ tư",
                DayOfWeek.Thursday => "Thứ năm",
                DayOfWeek.Friday => "Thứ sáu",
                DayOfWeek.Saturday => "Thứ bảy",
                DayOfWeek.Sunday => "Chủ nhật",
                _ => ""
            };
        }
    }
}
