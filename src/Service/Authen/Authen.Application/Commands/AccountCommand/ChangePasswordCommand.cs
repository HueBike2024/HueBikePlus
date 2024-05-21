using Core.Infrastructure.Handlers;
using Core.SeedWork.Repository;
using MediatR;
using Core.Interfaces.Database;
using Core.Exceptions;
using Core.Helpers.Cryptography;
using Core.Properties;
using Authen.Application.Constants;
using Infrastructure.AggregatesModel.Authen.AccountAggregate;
using Microsoft.EntityFrameworkCore;

namespace Authen.Application.Commands.AccountCommand
{
    public class ChangePasswordCommand : IRequest<bool>
    {
        public string PasswordOld { get; set; }
        public string PasswordNew { get; set; }
    }

    public class ChangePasswordCommandHandler : BaseHandler, IRequestHandler<ChangePasswordCommand, bool>
    {
        private readonly IRepository<User> _userRep;
        private readonly IUnitOfWork _unitOfWork;

        public ChangePasswordCommandHandler(IRepository<User> userRep, IUnitOfWork unitOfWork)
        {
            _userRep = userRep;
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRep.GetQuery(e => e.Id == UserId).FirstOrDefaultAsync(cancellationToken);

            if (user == null)
                throw new BaseException(ErrorsMessage.MSG_ERROR_LOGIN);

            if (!user.IsSuperAdmin)
            {
                // TODO: Gửi mail

                //if (!user.IsActive)
                //    throw new BaseException(Resource.MSG_VALID_BLOCK_AUTH);

                if (!user.IsActive)
                    throw new BaseException(ErrorsMessage.MSG_VALID_BLOCK_AUTH);
            }

            // Password
            //var decryptPasswordOldAes = AESHelper.DecryptAES(request.PasswordOld, AESConfig.Key, AESConfig.Iv);
            //var decryptPasswordNewAes = AESHelper.DecryptAES(request.PasswordNew, AESConfig.Key, AESConfig.Iv);

            if (!SHACryptographyHelper.ComparePasswords(request.PasswordOld, user.Password))
            {
                throw new BaseException(ErrorsMessage.MSG_ERROR_PASS_WORD);
            }

            if (request.PasswordOld == request.PasswordNew)
            {
                throw new BaseException(ErrorsMessage.MSG_VALID_PASS_WORD_NEW);
            }

            user.Password = SHACryptographyHelper.SHA256Encrypt(request.PasswordNew);

            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}
