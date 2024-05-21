namespace Core.Common
{
    public static class Constants
    {
        public const int DEFAULT_SPLIT_SIZE = 10;
        public const string DEFAULT_TIME_ZONE = "Asia/Ho_Chi_Minh";
        public const string DEFAULT_KEY_AES = "8080808080808080";
        public const string DEFAULT_IV_AES = "8080808080808080";
        public const string DEFAULT_FORMAT_PARENT_IDS = "-{0}!";
        public static class NumberFormat
        {
            public const string Percentage = "0.0%";
            public const string Number = "0.0";
            public const string Decimal = "#,##0.000";
            public const string Decimal1 = "#,##0.###";
            public const string Decimal2 = "#,##0.##";
            public const string Decimal3 = "#,##0.###";
            public const string Number1 = "#,##0";
            public const string Number2 = "0";
        }

        public struct CONFIG_KEYS
        {
            public const string APP_SETTING = "AppSettings";
            public const string JWT_TOKEN = "Jwt";
            public const string APP_CONNECTION_STRING = "MainDatabase";
            public const string SMTP_SETTING = "SmtpSetting";
            public const string MEDIA_SETTING = "MediaSetting";
            public const string GOOGLE_SETTING = "GoogleSetting";
            public const string FACEBOOK_SETTING = "FacebookSetting";
            public const string FIREBASE_SETTING = "FirebaseSetting";
            public const string AGGREGATOR_BFF_SETTING = "AggregatorBff";
            public const string MAIL_SETTING = "MailSetting";
            public const string APPLICATION_URL = "ApplicationUrl";
            public const string MINIO = "MinIO";
        }
        public struct VERTICAL_ALIGNMENTS
        {
            public const string TOP = "Top";
            public const string CENTER = "Center";
            public const string BOTTOM = "Bottom";
            public const string DISTRIBUTED = "Distributed";
            public const string JUSTIFY = "Justify";
        }

        public struct HORIZONT_ALALIGNMENTS
        {
            public const string GENERAL = "General";
            public const string LEFT = "Left";
            public const string CENTER = "Center";
            public const string CENTER_CONTINUOUS = "CenterContinuous";
            public const string RIGHT = "Right";
            public const string FILL = "Fill";
            public const string DISTRIBUTED = "Distributed";
            public const string JUSTIFY = "Justify";
        }
    }
}
