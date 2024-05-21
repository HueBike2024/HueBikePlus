using Core.Exceptions;
using Core.Helpers.Cryptography;
using Core.Infrastructure.Handlers;
using Core.Interfaces.Database;
using Core.Properties;
using Core.SeedWork.Repository;
using Infrastructure.AggregatesModel.Authen.AccountAggregate;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using User = Infrastructure.AggregatesModel.Authen.AccountAggregate.User;
using MasterData.Application.Constants;

namespace MasterData.Application.Commands.UserCommand
{
    public class GrantPasswordCommand : IRequest<bool>
    {
        public long UserId { get; set; }
    }

    public class GrantPasswordCommandHandler : IRequestHandler<GrantPasswordCommand, bool>
    {
        public readonly IRepository<User> _userRep;
        public readonly IUnitOfWork _unitOfWork;

        public GrantPasswordCommandHandler(IRepository<User> userRep, IUnitOfWork unitOfWork)
        {
            _userRep = userRep;
            _unitOfWork = unitOfWork;
        }
        public async Task<bool> Handle(GrantPasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRep.FindOneAsync(e => e.Id == request.UserId);
            if (user == null)
            {
                throw new BaseException("Không tìm thấy người dùng!");
            }

            MailMessage mail = new MailMessage(EmailConfig.FromEmail, user.Email);

            mail.Subject = "Cấp lại mật khẩu";

            mail.Body = $"Mật khẩu mới vừa được cấp lại là: {EmailConfig.PasswordReset}";

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

                User.ResetPassword(ref user, passwordResetSHA);

                await _unitOfWork.SaveChangesAsync();
            }

            return true;
        }
    }
}
