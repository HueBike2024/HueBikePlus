using Core.Models.Emails;

namespace Core.Interfaces.Email
{
    public interface IEmailService
    {
        /// <summary>
        /// Send email
        /// </summary>
        /// <param name="emailModel"></param>
        void Send(EmailModel email);
    }
}
