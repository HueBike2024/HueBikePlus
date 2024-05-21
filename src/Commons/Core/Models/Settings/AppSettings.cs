namespace Core.Models.Settings
{
    public class AppSettings
    {
        public string[] AllowedHosts { get; set; }
        public JwtTokenSetting Jwt { get; set; }
    }
}
