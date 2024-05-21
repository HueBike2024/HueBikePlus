using Core.Helpers;

namespace Core.Common
{
    public class AppConstants
    {
        public const string MainConnectionString = "MainDatabase";
        public const string HangfireSchemaName = "HangfireSchemaName";        
        public const string SuperAdminRole = "Admin";
        public const string UserRole = "User";

        public struct Modules
        {
            public const string AUTHEN = "authen";
            public const string MASTERDATA = "master-data";
            public const string ECL = "ecl";            
        }

        public struct ContentTypes
        {
            public const int MAX_FILE_SIZE = 209715200;
            public const string EXEL_FILE = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            public const string APPLICATION_JSON = "application/json";
            public const string TEXT_PLAIN = "text/plain";
            public const string APPLICATION_OCTECT_STREAM = "application/octet-stream";
        }

        public static class Auth
        {
            public const string Authorization = "Authorization";
            public const string BearerHeader = "Bearer ";
            public const string IP = "IP";
            public const string TimezoneOffset = "TimezoneOffset";
            /// <summary>
            /// Access Token
            /// </summary>
            public const string AccessToken = "access_token";

            /// <summary>
            /// Refresh Token
            /// </summary>
            public const string RefreshToken = "refresh_token";
        }

        /// <summary>
        /// Claim Type
        /// </summary>
        public static class ClaimType
        {
            /// <summary>
            /// UserId
            /// </summary>
            public const string UserId = "UserId";
            /// <summary>
            /// User name
            /// </summary>
            public const string UserName = "UserName";
            /// <summary>
            /// IP
            /// </summary>
            public const string IP = "IP";
            /// <summary>
            /// IsSuperAdmin
            /// </summary>
            public const string IsSuperAdmin = "IsSuperAdmin";
            /// <summary>
            /// Time Zone
            /// </summary>
            public const string TimeZone = "TimeZone";
        }    

        public static class DateTimeFormat
        {
            public const string FullDateTime = "dd/MM/yyyy HH:mm:ss";
            public const string DateTimeIsoString = "yyyy-MM-dd'T'HH:mm:ss.fffZ";
            public const string DateTimeLocalString = "yyyy-MM-dd HH:mm:ss.fff";
            public const string DateString = "dd/MM/yyyy";
            public const string DateFormatString = "yyyy-MM-dd";
            public const string TimeString = "HH:mm";
            public const string DateTimeString = "yyyy-MM-dd'T'HH:mm:ss";
            public const string DateTimeFormatString = "dd-MM-yyyy";
            public const string DateTimeHourMinuteSecondString = "dd-MM-yyyy-hh-mm-ss";
            public const string FILE_YYMMDDHHMMSS = "yyMMddhhmmss";

            public const string Hour = "HH:mm";
        }

        // TODO
        public static class Permissions
        {
            public const string Owner = "*";
            public const string Manager = "m";
            public const string Editor = "e";
            public const string Viewer = "v";
        }

        public static class LevelPermision
        {
            public const int Owner = 1;
            public const int Manager = 2;
            public const int Editor = 3;
            public const int Viewer = 4;
        }

        public static class NameLevelPermision
        {
            public const string Owner = "Personal";
            public const string Manager = "Manage";
            public const string Editor = "Editor";
            public const string Viewer = "Viewer";
        }

        public static Dictionary<string, int> DicLevelPermision = new()
        {
            {Permissions.Owner      ,1},
            {Permissions.Manager    ,2},
            {Permissions.Editor     ,3},
            {Permissions.Viewer     ,4},
        };

        public static Dictionary<int, string> DicNameLevelPermision = new()
        {
            {LevelPermision.Owner  ,NameLevelPermision.Owner},
            {LevelPermision.Manager  ,NameLevelPermision.Manager},
            {LevelPermision.Editor  ,NameLevelPermision.Editor},
            {LevelPermision.Viewer  ,NameLevelPermision.Viewer},
        };
    }
}
