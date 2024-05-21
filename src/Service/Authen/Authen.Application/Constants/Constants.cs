namespace Authen.Application.Constants
{
    public class Constants
    {
        public const int TokenLifeTime = 10080;
        public const int RefreshTokenLifeTime = 43200;
    }

    public static class AESConfig
    {
        public const string Key = "8080808080808080";
        public const string Iv = "8080808080808080";
    }

    public static class EmailConfig
    {
        public const string PasswordReset = "1212ab@";
        public const string FromEmail = "20t1080031@husc.edu.vn";
        public const string Password = "nahoshi";
    }
}
