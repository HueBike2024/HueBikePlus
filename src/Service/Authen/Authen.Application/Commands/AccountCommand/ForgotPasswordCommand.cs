using Authen.Application.Constants;
using Core.Exceptions;
using Core.Helpers.Cryptography;
using Core.Interfaces.Database;
using Core.Properties;
using Core.SeedWork.Repository;
using Infrastructure.AggregatesModel.Authen.AccountAggregate;
using MediatR;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Net.Mail;

namespace Authen.Application.Commands.AccountCommand
{
    public class ForgotPasswordCommand: IRequest<bool>
    {
        public string Username { get; set; }
        [EmailAddress(ErrorMessage = "Email incorrect formatted")] //Todo: Sau này sẽ làm mã hóa thông tin 
        public string Email { get; set; }
    }

    public class ForgotPasswordCommandHandler: IRequestHandler<ForgotPasswordCommand, bool>
    {
        private readonly IRepository<User> _userRep;

        private readonly IUnitOfWork _unitOfWork;

        public ForgotPasswordCommandHandler(IRepository<User> userRep, IUnitOfWork unitOfWork)
        {
            _userRep = userRep;
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
        {
            MailMessage mail = new MailMessage(EmailConfig.FromEmail, request.Email);

            mail.Subject = "Đặt lại mật khẩu của bạn";

            mail.Body = $"Mật khẩu mới của bạn là: {EmailConfig.PasswordReset}";

            var isSendMail = true;

            try
            {
                SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);

                smtpClient.Credentials = new NetworkCredential(EmailConfig.FromEmail, EmailConfig.Password);

                smtpClient.EnableSsl = true;

                smtpClient.Send(mail);
            }
            catch (Exception ex)
            {
                isSendMail = false;
            }

            if (isSendMail)
            {
                var passwordResetSHA = SHACryptographyHelper.SHA256Encrypt(EmailConfig.PasswordReset);

                var user = await _userRep.FindOneAsync(e => e.UserName == request.Username);

                if (user == null)
                {
                    throw new BaseException(ErrorsMessage.MSG_NOT_EXIST, "Username");
                }

                User.ResetPassword(ref user, passwordResetSHA);

                await _unitOfWork.SaveChangesAsync();
            }

            return isSendMail;
        }
    }
}
