using Core.Exceptions;
using Core.Helpers.Cryptography;
using Core.Interfaces.Database;
using Core.Properties;
using Core.SeedWork.Repository;
using Infrastructure.AggregatesModel.Authen.AccountAggregate;
using MasterData.Application.Commands.UnitCommand;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User = Infrastructure.AggregatesModel.Authen.AccountAggregate.User;

namespace MasterData.Application.Commands.UserCommand
{
    public class CreateUserCommand : IRequest<bool>
    {
        public long? UserId { get; set; }
        public string FullName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string UserName { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string? Password { get; set; }
        public string? PasswordConfirm { get; set; }
        public bool IsSuperAdmin { get; set; }
    }

    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, bool>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public readonly IRepository<User> _userRep;
        public readonly IUnitOfWork _unitOfWork;

        public CreateUserCommandHandler(IHttpContextAccessor httpContextAccessor, IRepository<User> userRep, IUnitOfWork unitOfWork)
        {
            _httpContextAccessor = httpContextAccessor;
            _userRep = userRep;
            _unitOfWork = unitOfWork;
        }
        public async Task<bool> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            // Lấy đối tượng HttpContext từ IHttpContextAccessor
            var httpContext = _httpContextAccessor.HttpContext;

            // Kiểm tra giao thức HTTP
            if (httpContext.Request.Method == HttpMethods.Post && (request.UserId == null || request.UserId == 0))
            {
                var existUser = await _userRep.GetQuery(e => e.UserName == request.UserName
                                        || (!string.IsNullOrEmpty(request.Email) && e.Email == request.Email) || (!string.IsNullOrEmpty(request.PhoneNumber) && e.PhoneNumber == request.PhoneNumber))
                                        .IgnoreQueryFilters()
                                        .AsNoTracking()
                                        .FirstOrDefaultAsync(cancellationToken);

                if (existUser != null)
                {
                    if (existUser.UserName == request.UserName)
                        throw new BaseException(ErrorsMessage.MSG_EXIST, "Tên đăng nhập");

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

                if (string.IsNullOrEmpty(request.UserName))
                {
                    throw new BaseException(ErrorsMessage.MSG_REQUIRED, "Tên đăng nhập");
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

                if (string.IsNullOrEmpty(request.Password))
                {
                    throw new BaseException(ErrorsMessage.MSG_REQUIRED, "Mật khẩu");
                }

                if (string.IsNullOrEmpty(request.PasswordConfirm))
                {
                    throw new BaseException(ErrorsMessage.MSG_REQUIRED, "Mật khẩu xác nhận");
                }

                if (request.Password != request.PasswordConfirm)
                {
                    throw new BaseException(ErrorsMessage.MSG_FAILED, "Mật khẩu xác nhận không trùng khớp, ");
                }

                if (request.IsSuperAdmin == null)
                {
                    request.IsSuperAdmin = false;
                }

                string password = SHACryptographyHelper.SHA256Encrypt(request.Password);

                var user = new User(request.FullName, request.UserName, request.Address, request.Email, request.PhoneNumber, password/*, null*/);

                user.IsActive = true;
                user.IsLock = false;
                user.DateOfBirth = request.DateOfBirth;
                user.IsSuperAdmin = request.IsSuperAdmin;


                _userRep.Add(user);

                await _unitOfWork.SaveChangesAsync();

                return true;
            }
            else
            {
                var user = await _userRep.FindOneAsync(e => e.Id == request.UserId);
                if (user == null)
                {
                    throw new BaseException("Không tìm thấy người dùng!");
                }

                var existUser = await _userRep.GetQuery(e => e.Id != request.UserId && (e.UserName == request.UserName
                                            || (!string.IsNullOrEmpty(request.Email) && e.Email == request.Email) || (!string.IsNullOrEmpty(request.PhoneNumber) && e.PhoneNumber == request.PhoneNumber)))
                                            .IgnoreQueryFilters()
                                            .AsNoTracking()
                                            .FirstOrDefaultAsync(cancellationToken);

                if (existUser != null)
                {
                    if (existUser.UserName == request.UserName)
                        throw new BaseException(ErrorsMessage.MSG_EXIST, "Tên đăng nhập");

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

                if (string.IsNullOrEmpty(request.UserName))
                {
                    throw new BaseException(ErrorsMessage.MSG_REQUIRED, "Tên đăng nhập");
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

                if (request.IsSuperAdmin == null)
                {
                    request.IsSuperAdmin = false;
                }

                user.FullName = request.FullName;
                user.DateOfBirth = request.DateOfBirth;
                user.UserName = request.UserName;
                user.Address = request.Address;
                user.PhoneNumber = request.PhoneNumber;
                user.Email = request.Email;
                user.IsSuperAdmin = request.IsSuperAdmin;
                user.UpdatedDate = DateTime.UtcNow;

                _userRep.Update(user);

                await _unitOfWork.SaveChangesAsync();

                return true;
            }
        }
    }
}
