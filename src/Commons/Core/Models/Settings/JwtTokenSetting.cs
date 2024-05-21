namespace Core.Models.Settings
{
    public class JwtTokenSetting
    {
        public string SecretKey { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public int TokenLifeTimeForMobile { get; set; }
        public int RefreshTokenLifeTimeMobile { get; set; }
        public int TokenLifeTimeForWeb { get; set; }
        public int RefreshTokenLifeTimeWeb { get; set; }
        public int PasswordLength { get; set; }
        public int LoginMaxTry { get; set; } = 5;
        public int LockoutTimeInMinutes { get; set; } = 5;
        public int ResetPasswordExpireTime { get; set; }
        public int ExpiryMinutes { get; set; }
        public int PasswordMinutes { get; set; }
        public int TokenExtension { get; set; }
    }
}
