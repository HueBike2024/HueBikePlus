using Core.Exceptions;
using Core.Helpers.Cryptography;
using Core.Interfaces.Logging;
using Core.Models.Emails;
using Core.Properties;
using Core.SeedWork.Repository;
using Infrastructure.AggregatesModel.System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;

namespace Infrastructure.Services
{
    public interface IEmailService
    {
        /// <summary>
        /// Send email
        /// </summary>
        /// <param name="emailModel"></param>
        void Send(EmailModel email);
    }
    public class EmailService : IEmailService
    {
        private readonly IAppLogger _appLogger;
        private readonly IConfiguration _configuration;
        public delegate void SendMailHandler(EmailModel email);
        private readonly IRepository<EmailServer> _emailRep;

        public EmailService(IAppLogger appLogger, IConfiguration configuration, IRepository<EmailServer> emailRep)
        {
            _appLogger = appLogger;
            _configuration = configuration;
            _emailRep = emailRep;
        }
        public void Send(EmailModel email)
        {
            Task.Run(() =>
            {
                var handler = new SendMailHandler(SendMailBackground);
                handler(email);
            });
        }
        private async void SendMailBackground(EmailModel email)
        {
            try
            {
                var smtpSetting = await _emailRep.GetQuery().FirstOrDefaultAsync();
                if (smtpSetting == null)
                {
                    throw new BaseException(string.Format(ErrorsMessage.MSG_NOT_EXIST, "Email server"));
                }

                var smtpClient = new SmtpClient
                {
                    Host = smtpSetting.Host,
                    EnableSsl = smtpSetting.EnableSsl,
                    Port = Convert.ToInt32(smtpSetting.Port),
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = smtpSetting.DefaultCredentials,
                    Credentials = new BasicNetworkCredential(
                        AESHelper.DecryptAES(smtpSetting.Account, null, null),
                        AESHelper.DecryptAES(smtpSetting.Password, null, null)),
                    //Timeout = 60000
                };

                var fromAddress = new MailAddress(AESHelper.DecryptAES(smtpSetting.Account, null, null), smtpSetting.SenderName);
                var mailMessage = new MailMessage()
                {
                    From = fromAddress,
                    Subject = email.Subject,
                    IsBodyHtml = true,
                    Body = email.Body,
                    BodyEncoding = Encoding.UTF8,
                    SubjectEncoding = Encoding.UTF8
                };

                // Create the HTML view
                var htmlView = AlternateView.CreateAlternateViewFromString(email.Body, Encoding.UTF8, MediaTypeNames.Text.Html);

                //var imageLinked = EmailLogoLinkedResource();
                //htmlView.LinkedResources.Add(imageLinked);

                mailMessage.AlternateViews.Add(htmlView);

                foreach (var toEmail in email.ToAddresses)
                {
                    mailMessage.To.Add(toEmail);
                }

                if (email.Attachment != null)
                {
                    Stream stream = new MemoryStream(email.Attachment.File);
                    var attachment = new Attachment(stream, email.Attachment.FileName, System.Net.Mime.MediaTypeNames.Application.Octet);

                    if (!string.IsNullOrEmpty(email.Attachment.FileType))
                        attachment.ContentType = new System.Net.Mime.ContentType(email.Attachment.FileType);

                    mailMessage.Attachments.Add(attachment);
                }

                smtpClient.Send(mailMessage);
                mailMessage.Dispose();
            }
            catch (Exception ex)
            {
                _appLogger.Error(ex.Message);
            }
        }
        private class BasicNetworkCredential : ICredentialsByHost
        {
            private readonly NetworkCredential _wrappedNetworkCredential;
            public BasicNetworkCredential(string username, string password)
            {
                _wrappedNetworkCredential = new NetworkCredential(username, password);
            }

            public NetworkCredential GetCredential(string host, int port, string authenticationType)
            {
                return authenticationType.ToLower() != "login" ? null : _wrappedNetworkCredential.GetCredential(host, port, authenticationType);
            }
        }
    }
}
