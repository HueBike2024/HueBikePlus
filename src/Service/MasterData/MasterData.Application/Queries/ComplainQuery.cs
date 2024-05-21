using Azure.Core;
using Core.Exceptions;
using Core.Properties;
using Core.SeedWork;
using Core.SeedWork.Repository;
using Infrastructure.AggregatesModel.Authen.AccountAggregate;
using Infrastructure.AggregatesModel.MasterData.UserAggregate.AuthenticationAggregate;
using Infrastructure.AggregatesModel.MasterData.UserAggregate.ComplainAggregate;
using MasterData.Application.Commands.ComplainCommand;
using MasterData.Application.Commands.UserCommand;
using MasterData.Application.DTOs.Complain;
using MasterData.Application.DTOs.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterData.Application.Queries
{
    public interface IComplainQuery
    {
        /// <summary>
        /// Chi tiết 1 complain
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<ComplainDetailResponse> GetAsync(DetailComplainCommand command);
        /// <summary>
        /// Danh sách complain
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<PagingResultSP<ComplainResponse>> ListAsync(ListComplainCommand command);
    }

    public class ComplainQuery : IComplainQuery
    {
        private readonly IRepository<Complain> _compRep;
        private readonly IRepository<ComplainReply> _replyRep;
        private readonly IRepository<User> _userRep;
        public ComplainQuery(IRepository<Complain> complainRep, IRepository<ComplainReply> replyRep, IRepository<User> userRep)
        {
            _userRep = userRep;
            _compRep = complainRep;
            _replyRep = replyRep;

        }

        public async Task<ComplainDetailResponse> GetAsync(DetailComplainCommand request)
        {
            var user = await _userRep.FindOneAsync(e => e.Id == request.SenderId);
            var complain = await _compRep.FindOneAsync(e => e.Id == request.ComplainId && e.SenderId == request.SenderId);

            if (user == null)
            {
                throw new BaseException("Không tìm thấy Người dùng");
            }

            if (complain == null)
            {
                throw new BaseException("Không tìm thấy Khiếu nại!");
            }

            // Lấy thông tin về complaint
            var complainResult = await _compRep.GetQuery(e => e.Id == request.ComplainId)
                .Select(k => new ComplainDetailResponse
                {
                    Id = k.Id,
                    SenderId = request.SenderId,
                    SenderUsername = user.UserName,
                    Image = k.Image,
                    Content = k.Content,
                    CreatedDate = k.CreatedDate,
                    Replys = new List<Reply>()
                }).FirstOrDefaultAsync();

            // Kiểm tra xem complaint có tồn tại không
            if (complainResult != null)
            {
                // Lấy danh sách các reply từ _replyRep
                var replies = await _replyRep.GetQuery().Where(e => e.ComplainId == request.ComplainId).ToListAsync();

                foreach (var rep in replies)
                {
                    complainResult.Replys.Add(new Reply
                    {
                        Id = rep.Id,
                        ComplainId= rep.ComplainId,
                        SenderId = rep.SenderId,
                        Content = rep.Content,
                        CreatedDate = rep.CreatedDate,
                    });
                }

            }

            return complainResult;

        }

        public async Task<PagingResultSP<ComplainResponse>> ListAsync(ListComplainCommand request)
        {
            var query = _compRep.GetQuery();

            if (!string.IsNullOrEmpty(request.SearchTerm))
            {
                request.SearchTerm = request.SearchTerm.ToLower().Trim();
                // Thử chuyển đổi SearchTerm sang long
                long searchTermAsLong;
                bool isNumeric = long.TryParse(request.SearchTerm, out searchTermAsLong);

                query = query.Where(e =>
                    e.ComplainSender.UserName.ToLower().Contains(request.SearchTerm) ||
                    e.Id == searchTermAsLong || // So sánh với ID dạng long
                    e.SenderId == searchTermAsLong || // So sánh với ID dạng long
                    (isNumeric && e.Id == searchTermAsLong) // Kiểm tra nếu SearchTerm có thể chuyển thành long
                );
            }

            var complainResponse = query.Select(e => new ComplainResponse
            {
                Id = e.Id,
                SenderId= e.SenderId,
                SenderUsername = e.ComplainSender.UserName,
                Image = e.Image,
                Content = e.Content,
                CreatedDate = e.CreatedDate,
            });

            if (string.IsNullOrEmpty(request.OrderBy) && string.IsNullOrEmpty(request.OrderByDesc))
            {
                complainResponse = complainResponse.OrderByDescending(e => e.CreatedDate);
            }
            else
            {
                complainResponse = PagingSorting.Sorting(request, complainResponse);
            }
            var pageIndex = request.PageSize * (request.PageIndex - 1);

            var response = await PaginatedList<ComplainResponse>.CreateAsync(complainResponse, pageIndex, request.PageSize);

            var result = new PagingResultSP<ComplainResponse>(response, response.Total, request.PageIndex, request.PageSize);
            var i = pageIndex + 1;

            foreach (var item in result.Data)
            {
                item.Index = i++;
            }

            return result;

        }
    }
}
