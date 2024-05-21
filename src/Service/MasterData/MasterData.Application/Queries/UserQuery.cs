using Core.SeedWork.Repository;
using Core.SeedWork;
using MasterData.Application.Commands.UnitCommand;
using MasterData.Application.DTOs.Unit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MasterData.Application.DTOs.User;
using MasterData.Application.Commands.UserCommand;
using Infrastructure.AggregatesModel.Authen.AccountAggregate;
using Microsoft.EntityFrameworkCore;
using Infrastructure.AggregatesModel.MasterData.UserAggregate.AuthenticationAggregate;
using Core.Exceptions;
using Infrastructure.AggregatesModel.MasterData.ImageAggregate;
using Core.Infrastructure.Handlers;

namespace MasterData.Application.Queries
{
    public interface IUserQuery
    {
        /// <summary>
        /// Chi tiết 1 người dùng
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<UserDetailResponse> GetAsync(UserDetailCommand command);
        /// <summary>
        /// Danh sách người dùng
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<PagingResultSP<UserResponse>> ListAsync(ListUserCommand command);
    }
    public class UserQuery : BaseHandler, IUserQuery
    {
        private readonly IRepository<User> _userRep;
        private readonly IRepository<UserAuthentication> _authenRep;
        private readonly IRepository<Avatar> _avatarRep;
        public UserQuery(IRepository<User> userRep, IRepository<UserAuthentication> authenRep, IRepository<Avatar> avatarRep)
        {
            _userRep = userRep;
            _authenRep = authenRep;
            _avatarRep = avatarRep;

        }

        public async Task<UserDetailResponse> GetAsync(UserDetailCommand request)
        {
            var user = await _userRep.FindOneAsync(e => e.Id == request.UserId);
            var userauthen = await _authenRep.FindOneAsync(e => e.Id == user.AuthenId);
            var avatar = await _avatarRep.FindOneAsync(e => e.Id == user.PhotoId);
            var avatarUrl = "";
            if(avatar != null)
            {
                avatarUrl = avatar.ImageUrl;
            }

            if (user.AuthenId == null)
            {
                return await _userRep.GetQuery(e => e.Id == request.UserId)
                .Select(k => new UserDetailResponse
                {
                    FullName = k.FullName,
                    DateOfBirth = k.DateOfBirth,
                    AvatarUrl = avatarUrl,
                    Address = k.Address,
                    Email = k.Email,
                    CardId = null,
                    PhoneNumber = k.PhoneNumber,
                    IsConfirm = k.IsConfirm,
                    CreatedDate = k.CreatedDate,
                    UpdatedDate = k.UpdatedDate,
                }).FirstOrDefaultAsync();
                //throw new BaseException("Người dùng này chưa xác thực thông tin");
            }

            return await _userRep.GetQuery(e => e.Id == request.UserId)
                .Select(k => new UserDetailResponse
                {
                    FullName = k.FullName,
                    DateOfBirth = k.DateOfBirth,
                    AvatarUrl = avatarUrl,
                    Address = k.Address,
                    Email = k.Email,
                    CardId = userauthen.CardId,
                    PhoneNumber = k.PhoneNumber,
                    IsConfirm = k.IsConfirm,
                    CreatedDate = k.CreatedDate,
                    UpdatedDate = k.UpdatedDate,
                }).FirstOrDefaultAsync();
        }

        public async Task<PagingResultSP<UserResponse>> ListAsync(ListUserCommand request)
        {
            var query = _userRep.GetQuery();

            if (!string.IsNullOrEmpty(request.SearchTerm))
            {
                request.SearchTerm = request.SearchTerm.ToLower().Trim();
                // Thử chuyển đổi SearchTerm sang long
                long searchTermAsLong;
                bool isNumeric = long.TryParse(request.SearchTerm, out searchTermAsLong);

                query = query.Where(e =>
                    e.FullName.ToLower().Contains(request.SearchTerm) ||
                    e.UserName.ToLower().Contains(request.SearchTerm) ||
                    e.Email.ToLower().Contains(request.SearchTerm) ||
                    e.PhoneNumber.Contains(request.SearchTerm) ||
                    e.Id == searchTermAsLong || // So sánh với ID dạng long
                    (isNumeric && e.Id == searchTermAsLong) // Kiểm tra nếu SearchTerm có thể chuyển thành long
                );
            }

            var userResponse = query.Select(e => new UserResponse
            {
                Id = e.Id,
                FullName = e.FullName,
                UserName = e.UserName,
                DateOfBirth = e.DateOfBirth,
                AvatarId = e.PhotoId,
                StatusId = e.StatusId,
                IsActive = e.IsActive,
                IsLock = e.IsLock,
                IsConfirm = e.IsConfirm,
                Address = e.Address,
                Email = e.Email,
                PhoneNumber = e.PhoneNumber,
                CreatedDate = e.CreatedDate,
            });

            if (string.IsNullOrEmpty(request.OrderBy) && string.IsNullOrEmpty(request.OrderByDesc))
            {
                userResponse = userResponse.OrderByDescending(e => e.CreatedDate);
            }
            else
            {
                userResponse = PagingSorting.Sorting(request, userResponse);
            }
            var pageIndex = request.PageSize * (request.PageIndex - 1);

            var response = await PaginatedList<UserResponse>.CreateAsync(userResponse, pageIndex, request.PageSize);

            var result = new PagingResultSP<UserResponse>(response, response.Total, request.PageIndex, request.PageSize);
            var i = pageIndex + 1;

            foreach (var item in result.Data)
            {
                item.Index = i++;
            }

            return result;

        }
    }
}
