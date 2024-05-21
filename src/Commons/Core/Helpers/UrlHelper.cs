using Core.Models;
using System.Collections.Generic;
using System.Linq;

namespace Core.Helpers
{
    public static class UrlHelper
    {
        public static string ConvertListToParams(List<UrlParamModel> items)
        {
            return string.Join("&", items.Select(x => $"{x.Key}={x.Value}"));
        }
        public static string ConvertListToParams(string paramName, List<string> items)
        {
            return string.Join("&", items.Select(x => $"{paramName}={x}"));
        }
    }
}
