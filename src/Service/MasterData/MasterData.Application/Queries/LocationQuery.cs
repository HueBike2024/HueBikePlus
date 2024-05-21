using Core.SeedWork;
using Core.SeedWork.Repository;
using Infrastructure.AggregatesModel.MasterData.BikeManagementAggregate.MapLocationAggregate;
using MasterData.Application.Commands.LocationCommand;
using MasterData.Application.Commands.StatusCommand;
using MasterData.Application.Commands.UnitCommand;
using MasterData.Application.DTOs.Location;
using MasterData.Application.DTOs.Status;
using MasterData.Application.DTOs.Unit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterData.Application.Queries
{
    public interface ILocationQuery
    {
        /// <summary>
        /// Danh sách complain
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<PagingResultSP<LocationResponse>> ListAsync(ListLocationCommand command);
    }

    public class LocationQuery : ILocationQuery
    {
        private readonly IRepository<MapLocation> _locationRep;
        public LocationQuery(IRepository<MapLocation> locationRep)
        {
            _locationRep = locationRep;
        }

        public async Task<PagingResultSP<LocationResponse>> ListAsync(ListLocationCommand request)
        {
            var query = _locationRep.GetQuery();

            if (!string.IsNullOrEmpty(request.SearchTerm))
            {
                request.SearchTerm = request.SearchTerm.ToLower().Trim();
                // Thử chuyển đổi SearchTerm sang long
                long searchTermAsLong;
                bool isNumeric = long.TryParse(request.SearchTerm, out searchTermAsLong);

                request.SearchTerm = request.SearchTerm.ToLower().Trim();
                query = query.Where(e =>
                    e.LocationName.ToLower().Contains(request.SearchTerm) ||
                    e.Id == searchTermAsLong || // So sánh với ID dạng long
                    (isNumeric && e.Id == searchTermAsLong)
                );// Kiểm tra nếu SearchTerm có thể chuyển thành long
            }
            var locationResponse = query.Select(e => new LocationResponse
            {
                Id = e.Id,
                LocationName = e.LocationName,
                CreatedDate = e.CreatedDate,
                UpdatedDate = e.UpdatedDate,
                Logitude = e.Longitude,
                Latitude = e.Latitude

            });

            if (string.IsNullOrEmpty(request.OrderBy) && string.IsNullOrEmpty(request.OrderByDesc))
            {
                locationResponse = locationResponse.OrderByDescending(e => e.CreatedDate);
            }
            else
            {
                locationResponse = PagingSorting.Sorting(request, locationResponse);
            }
            var pageIndex = request.PageSize * (request.PageIndex - 1);

            var response = await PaginatedList<LocationResponse>.CreateAsync(locationResponse, pageIndex, request.PageSize);

            var result = new PagingResultSP<LocationResponse>(response, response.Total, request.PageIndex, request.PageSize);
            var i = pageIndex + 1;

            foreach (var item in result.Data)
            {
                item.Index = i++;
            }

            return result;

        }
    }
}
