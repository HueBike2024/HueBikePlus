using Core.Exceptions;
using Core.Helpers.Cryptography;
using Core.Infrastructure.Handlers;
using Core.Interfaces.Database;
using Core.Properties;
using Core.SeedWork.Repository;
using Infrastructure.AggregatesModel.Authen.AccountAggregate;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterData.Application.Commands.UserCommand
{
    public class UpdateUserInfoCommand : IRequest<bool>
    {
        public string FullName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
    }

    public class UpdateUserInfoCommandHandler : BaseHandler, IRequestHandler<UpdateUserInfoCommand, bool>
    {
        public readonly IRepository<User> _userRep;
        public readonly IUnitOfWork _unitOfWork;

        public UpdateUserInfoCommandHandler(IRepository<User> userRep, IUnitOfWork unitOfWork)
        {
            _userRep = userRep;
            _unitOfWork = unitOfWork;
        }
        public async Task<bool> Handle(UpdateUserInfoCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRep.FindOneAsync(e => e.Id == UserId);
            if (user == null)
            {
                throw new BaseException("Không tìm thấy người dùng!");
            }

            var existUser = await _userRep.GetQuery(e => (e.Id != user.Id) && 
                                         ((!string.IsNullOrEmpty(request.Email) && e.Email == request.Email) || (!string.IsNullOrEmpty(request.Email) && e.Email == request.Email) || (!string.IsNullOrEmpty(request.PhoneNumber) && e.PhoneNumber == request.PhoneNumber)))
                                        .IgnoreQueryFilters()
                                        .AsNoTracking()
                                        .FirstOrDefaultAsync(cancellationToken);

            if (existUser != null)
            {
                if (existUser.Email == request.Email)
                    throw new BaseException(ErrorsMessage.MSG_EXIST, "Email");

                if (existUser.PhoneNumber == request.PhoneNumber)
                    throw new BaseException(ErrorsMessage.MSG_EXIST, "Số điện thoại");

                throw new BaseException(ErrorsMessage.MSG_EXIST, "Tài khoản");
            }

            if (string.IsNullOrEmpty(request.FullName))
            {
                throw new BaseException(ErrorsMessage.MSG_REQUIRED, "Họ và tên");
            }

            if (request.DateOfBirth == null)
            {
                throw new BaseException(ErrorsMessage.MSG_REQUIRED, "Ngày sinh");
            }

            if (string.IsNullOrEmpty(request.Address))
            {
                throw new BaseException(ErrorsMessage.MSG_REQUIRED, "Địa chỉ");
            }

            if (string.IsNullOrEmpty(request.PhoneNumber))
            {
                throw new BaseException(ErrorsMessage.MSG_REQUIRED, "Số điện thoại");
            }

            if (string.IsNullOrEmpty(request.Email))
            {
                throw new BaseException(ErrorsMessage.MSG_REQUIRED, "Email");
            }

            user.FullName = request.FullName;
            user.DateOfBirth = request.DateOfBirth;
            user.Address = request.Address;
            user.PhoneNumber = request.PhoneNumber;
            user.Email = request.Email;
            user.UpdatedDate = DateTime.UtcNow;

            _userRep.Update(user);

            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}
