using Core.Models.Interface;

namespace Infrastructure.AggregatesModel.System
{
    public class EmailServer : IEntity
    {
        public long Id { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
        public string Account { get; set; }
        public string Password { get; set; }
        public bool EnableSsl { get; set; }
        public string SenderName { get; set; }
        public bool DefaultCredentials { get; set; }

        public EmailServer()
        {

        }

        public EmailServer(string host, int port, string account, string password, bool enableSsl, string senderName, bool defaultCredentials)
        {
            Host = host;
            Port = port;
            Account = account;
            Password = password;
            EnableSsl = enableSsl;
            SenderName = senderName;
            DefaultCredentials = defaultCredentials;
        }
    }
}
