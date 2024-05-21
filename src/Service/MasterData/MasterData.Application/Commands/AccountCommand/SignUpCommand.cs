using Core.Exceptions;
using Core.Helpers.Cryptography;
using Core.Interfaces.Database;
using Core.Properties;
using Core.SeedWork.Repository;
using Infrastructure.AggregatesModel.Authen.AccountAggregate;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace MasterData.Application.Commands.AccountCommand
{
    public class SignUpCommand: IRequest<bool>
    {
        public string FullName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        [EmailAddress(ErrorMessage = "Email incorrect formatted")] //Todo: sau này sẽ update mã hóa thông tin 
        public string Email { get; set; }
        [Phone(ErrorMessage = "Phone number incorrect formatted")] //Todo: sau này sẽ update mã hóa thông tin 
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        // public string TimeZone { get; set; }
    }
    public class SignUpCommandHandler: IRequestHandler<SignUpCommand, bool>
    {
        private readonly IRepository<User> _userRep;
        private readonly IUnitOfWork _unitOfWork;

        public SignUpCommandHandler(IRepository<User> userRep,IUnitOfWork unitOfWork)
        {
            _userRep = userRep;
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(SignUpCommand request, CancellationToken cancellationToken)
        {
            var existUser = await _userRep.GetQuery(e => e.UserName == request.UserName
                                        || (!string.IsNullOrEmpty(request.Email) && e.Email == request.Email) || (!string.IsNullOrEmpty(request.PhoneNumber) && e.PhoneNumber == request.PhoneNumber))
                                        .IgnoreQueryFilters()
                                        .AsNoTracking()
                                        .FirstOrDefaultAsync(cancellationToken);

            if (existUser != null)
            {
                if (existUser.UserName == request.UserName)
                    throw new BaseException(ErrorsMessage.MSG_EXIST, "User name");

                if (existUser.Email == request.Email)
                    throw new BaseException(ErrorsMessage.MSG_EXIST, "Email");

                if (existUser.PhoneNumber == request.PhoneNumber)
                    throw new BaseException(ErrorsMessage.MSG_EXIST, "Phone number");

                throw new BaseException(ErrorsMessage.MSG_EXIST, "Account");
            }

            string password = SHACryptographyHelper.SHA256Encrypt(request.Password);

            var user = new User(request.FullName, request.UserName, request.Address, request.Email, request.PhoneNumber, password /*request.TimeZone*/);

            user.IsActive = true;
            user.IsLock = false;

            _userRep.Add(user);

            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}
