using Core.SeedWork;
using Core.SeedWork.Repository;
using Unit = Infrastructure.AggregatesModel.MasterData.UnitAggregate.Unit;
using MasterData.Application.Commands.UnitCommand;
using MasterData.Application.DTOs.Unit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace MasterData.Application.Queries
{
    public interface IUnitQuery
    {
        /// <summary>
        /// Chi tiết 1 đơn vị
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<UnitDetailResponse> GetAsync(GetUnitCommand command);
        /// <summary>
        /// Danh sách đơn vị
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<PagingResultSP<UnitResponse>> ListAsync(ListUnitCommand command);
    }
    public class UnitQuery : IUnitQuery
    {
        private readonly IRepository<Unit> _unitRep;
        public UnitQuery(IRepository<Unit> unitRep)
        {
            _unitRep = unitRep;
        }

        public async Task<UnitDetailResponse> GetAsync(GetUnitCommand request)
        {
            return await _unitRep.GetQuery(e => e.Id == request.Id)
                .Select(k => new UnitDetailResponse
                {
                    Id = k.Id,
                    Code = k.Code,
                    Name = k.Name,
                    Address = k.Address,
                    Email = k.Email,
                    PhoneNumber = k.PhoneNumber,
                    Fax = k.Fax,
                }).FirstOrDefaultAsync();
        }

        public async Task<PagingResultSP<UnitResponse>> ListAsync(ListUnitCommand request)
        {
            var query = _unitRep.GetQuery();

            if (!string.IsNullOrEmpty(request.SearchTerm))
            {
                request.SearchTerm = request.SearchTerm.ToLower().Trim();
                query = query.Where(e => e.Name.ToLower().Contains(request.SearchTerm)
                || e.Code.ToLower().Contains(request.SearchTerm)
                || e.Address.ToLower().Contains(request.SearchTerm)
                || e.Email.ToLower().Contains(request.SearchTerm)
                || e.PhoneNumber.Contains(request.SearchTerm));
            }
            var unitResponse = query.Select(e => new UnitResponse
            {
                Id = e.Id,
                Code = e.Code,
                Name = e.Name,
                Address = e.Address,
                Email = e.Email,
                PhoneNumber = e.PhoneNumber,
                Fax = e.Fax,
                CreatedDate = e.CreatedDate

            });

            if (string.IsNullOrEmpty(request.OrderBy) && string.IsNullOrEmpty(request.OrderByDesc))
            {
                unitResponse = unitResponse.OrderByDescending(e => e.CreatedDate);
            }
            else
            {
                unitResponse = PagingSorting.Sorting(request, unitResponse);
            }
            var pageIndex = request.PageSize * (request.PageIndex - 1);

            var response = await PaginatedList<UnitResponse>.CreateAsync(unitResponse, pageIndex, request.PageSize);

            var result = new PagingResultSP<UnitResponse>(response, response.Total, request.PageIndex, request.PageSize);
            var i = pageIndex + 1;

            foreach (var item in result.Data)
            {
                item.Index = i++;
            }

            return result;

        }
    }
}
