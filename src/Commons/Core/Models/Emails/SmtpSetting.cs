namespace Core.Models.Emails
{
    public class SmtpSetting
    {
        public string Host { get; set; }
        public string Port { get; set; }
        public string Account { get; set; }
        public string Password { get; set; }
        public bool EnableSsl { get; set; }
        public string SenderName { get; set; }
        public bool DefaultCredentials { get; set; }
    }
}
