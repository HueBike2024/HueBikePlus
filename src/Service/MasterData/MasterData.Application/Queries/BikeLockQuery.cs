using Core.SeedWork;
using Core.SeedWork.Repository;
using Infrastructure.AggregatesModel.MasterData.BikeManagementAggregate.BikeAggregate;
using MasterData.Application.Commands.BikeCommand;
using MasterData.Application.Commands.BikeLockCommand;
using MasterData.Application.Commands.UnitCommand;
using MasterData.Application.DTOs.Bike;
using MasterData.Application.DTOs.BikeLock;
using MasterData.Application.DTOs.Unit;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterData.Application.Queries
{
    public interface IBikeLockQuery
    {
        /// <summary>
        /// Chi tiết thông tin 1 khóa
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        Task<LockDetailResponse> GetAsync(GetLockCommand command);
        /// <summary>
        /// Danh sách khóa
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        Task<PagingResultSP<LockResponse>> ListAsync(ListBikelockCommand command);
    }
    public class BikeLockQuery : IBikeLockQuery
    {
        private readonly IRepository<BikeLock> _lockRep;
        public BikeLockQuery(IRepository<BikeLock> lockRep)
        {
            _lockRep = lockRep;
        }
        public async Task<LockDetailResponse> GetAsync(GetLockCommand request)
        {
            return await _lockRep.GetQuery(e => e.Id == request.Id)
                .Select(k => new LockDetailResponse
                {
                    Id = k.Id,
                    LockName = k.LockName,
                    PathQr = k.PathQr,
                    QrCodeImage = k.QrCodeImage,
                    Power = k.Power,
                    IsUsed = k.IsUsed                 
              }).FirstOrDefaultAsync();
        }

        public async Task<PagingResultSP<LockResponse>> ListAsync(ListBikelockCommand request)
        {
            var query = _lockRep.GetQuery();

            if (!string.IsNullOrEmpty(request.SearchTerm))
            {
                request.SearchTerm = request.SearchTerm.ToLower().Trim();
                query = query.Where(e => e.LockName.ToLower().Contains(request.SearchTerm));

            }
            var lockResponse = query.Select(e => new LockResponse
            {
                Id = e.Id,
                LockName = e.LockName,
                PathQr = e.PathQr,
                Power = e.Power,
                IsUsed = e.IsUsed,
                CreatedDate = e.CreatedDate

               

            });

            if (string.IsNullOrEmpty(request.OrderBy) && string.IsNullOrEmpty(request.OrderByDesc))
            {
                lockResponse = lockResponse.OrderByDescending(e => e.CreatedDate);
            }
            else
            {
                lockResponse = PagingSorting.Sorting(request, lockResponse);
            }
            var pageIndex = request.PageSize * (request.PageIndex - 1);

            var response = await PaginatedList<LockResponse>.CreateAsync(lockResponse, pageIndex, request.PageSize);

            var result = new PagingResultSP<LockResponse>(response, response.Total, request.PageIndex, request.PageSize);
            var i = pageIndex + 1;

            foreach (var item in result.Data)
            {
                item.Index = i++;
            }

            return result;

        }
    }
}
