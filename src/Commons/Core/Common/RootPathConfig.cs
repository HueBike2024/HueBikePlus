using System.IO;

namespace Core.Common
{
    public class RootPathConfig
    {
        private static readonly string Dirpath = Directory.GetCurrentDirectory();

        public class TemplatePath
        {
            public static readonly string Template = Dirpath + @"/Media/Templates/Excel/";
            public static readonly string Image = Dirpath + @"/Media/Images/";
        }
    }
}