using Core.SeedWork;
using Core.SeedWork.Repository;
using Infrastructure.AggregatesModel.MasterData.StatusAggregate;
using MasterData.Application.Commands.ComplainCommand;
using MasterData.Application.Commands.StatusCommand;
using MasterData.Application.Commands.UnitCommand;
using MasterData.Application.DTOs.Complain;
using MasterData.Application.DTOs.Status;
using MasterData.Application.DTOs.Unit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterData.Application.Queries
{
    public interface IStatusQuery
    {
        /// <summary>
        /// Danh sách complain
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<PagingResultSP<StatusResponse>> ListAsync(ListStatusCommand command);
    }

    public class StatusQuery : IStatusQuery
    {
        private readonly IRepository<Status> _statusRep;
        public StatusQuery(IRepository<Status> statusRep)
        {
            _statusRep = statusRep;
        }

        public async Task<PagingResultSP<StatusResponse>> ListAsync(ListStatusCommand request)
        {
            var query = _statusRep.GetQuery();

            if (!string.IsNullOrEmpty(request.SearchTerm))
            {
                request.SearchTerm = request.SearchTerm.ToLower().Trim();
                // Thử chuyển đổi SearchTerm sang long
                long searchTermAsLong;
                bool isNumeric = long.TryParse(request.SearchTerm, out searchTermAsLong);

                request.SearchTerm = request.SearchTerm.ToLower().Trim();
                query = query.Where(e =>
                    e.StatusName.ToLower().Contains(request.SearchTerm) ||
                    e.Id == searchTermAsLong || // So sánh với ID dạng long
                    (isNumeric && e.Id == searchTermAsLong)
                );// Kiểm tra nếu SearchTerm có thể chuyển thành long
            }
            var statusResponse = query.Select(e => new StatusResponse
            {
                Id = e.Id,
                StatusName = e.StatusName,
                CreatedDate = e.CreatedDate,
                UpdateDate = e.UpdatedDate,

            });

            if (string.IsNullOrEmpty(request.OrderBy) && string.IsNullOrEmpty(request.OrderByDesc))
            {
                statusResponse = statusResponse.OrderByDescending(e => e.CreatedDate);
            }
            else
            {
                statusResponse = PagingSorting.Sorting(request, statusResponse);
            }
            var pageIndex = request.PageSize * (request.PageIndex - 1);

            var response = await PaginatedList<StatusResponse>.CreateAsync(statusResponse, pageIndex, request.PageSize);

            var result = new PagingResultSP<StatusResponse>(response, response.Total, request.PageIndex, request.PageSize);
            var i = pageIndex + 1;

            foreach (var item in result.Data)
            {
                item.Index = i++;
            }

            return result;

        }
    }
}
