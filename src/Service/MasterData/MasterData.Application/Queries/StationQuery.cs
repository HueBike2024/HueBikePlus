using Core.SeedWork;
using Core.SeedWork.Repository;
using Infrastructure.AggregatesModel.MasterData.BikeManagementAggregate.BikeAggregate;
using Infrastructure.AggregatesModel.MasterData.BikeManagementAggregate.StationAggregate;
using MasterData.Application.Commands.StationCommand;
using MasterData.Application.DTOs.Station;
using MasterData.Application.Services.StationService;
using Microsoft.EntityFrameworkCore;

namespace MasterData.Application.Queries
{
    public interface IStationQuery
    {
        /// <summary>
        /// Chi tiết thông tin 1 trạm
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        Task<StationDetailResponse> GetAsync(GetStationCommand command);
        /// <summary>
        /// Danh sách trạm
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<PagingResultSP<StationResponse>> ListAsync(ListStationCommand command);
        /// <summary>
        /// danh sách trạm theo thứ tự gần đến xa
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        Task<PagingResultSP<StationResponse>> ListStation(ListStationUserCommand command);
    }
    public class StationQuery : IStationQuery

    {
        private readonly IStationService _stationService;
        private readonly IRepository<Station> _stationRep;
        private readonly IRepository<Bike> _bikeRep;
        public StationQuery(IRepository<Station> stationRep, IRepository<Bike> bikeRep, IStationService stationService)
        {
            _stationRep = stationRep;
            _bikeRep = bikeRep;
            _stationService = stationService;
        }
        public async Task<StationDetailResponse> GetAsync(GetStationCommand request)
        {
            var station = await _stationRep.GetQuery(e => e.Id == request.Id)
         
         .Include(k => k.Status)   // Join với bảng Status
         .FirstOrDefaultAsync();

            if (station == null)
            {
                
            }

            // Đếm số lượng xe đang hoạt động và xe không hoạt đôngj
            var activeBikeCount = await _bikeRep.GetQuery(b => b.StationId == station.Id && b.StatusId == 1).CountAsync();

            var ortherBikeCount = await _bikeRep.GetQuery(b => b.StationId == station.Id && b.StatusId == 2).CountAsync();

            var totalAvailableBikes = activeBikeCount + ortherBikeCount;

            // Lấy danh sách các xe trong trạm
            var bikes = await _bikeRep.GetQuery(b => b.StationId == station.Id)
                .Select(b => new BikeList
                {
                    Id = b.Id,
                    BikeCode = b.BikeCode,                 
                    StatusName = b.Status.StatusName 
                }).ToListAsync();

            // Tạo StationDetailResponse
            var stationDetail = new StationDetailResponse
            {
                Id = station.Id,
                StationName = station.StationName,
                QuantityAvaiable = totalAvailableBikes,
                NumOfSeats = station.NumOfSeats,
                LocationName = station.LocationName,
                Longitude = station.Longitude,
                Latitude = station.Latitude,             
               
                StatusName = station.Status.StatusName,
                StatusId = station.Status.Id,
                
                NumOfActiveBikes = activeBikeCount, //  số lượng xe đang sử dụng
                NumOfOtherBikes = ortherBikeCount, // Số lượng xe chưa sử dụng
                Bikes = bikes

            };

            return stationDetail;
        }

        public async Task<PagingResultSP<StationResponse>> ListAsync(ListStationCommand request)
        {
            var query = _stationRep.GetQuery().Include(e => e.Status);

            if (!string.IsNullOrEmpty(request.SearchTerm))
            {
                request.SearchTerm = request.SearchTerm.ToLower().Trim();
              //  query = query.Where(e => e.StationName.ToLower().Contains(request.SearchTerm));
            }

            var stations = await query.ToListAsync();

            var stationResponses = stations.Select(e =>
            {
                var activeBikeCount = _bikeRep.GetQuery(b => b.StationId == e.Id && b.StatusId == 1).Count();
                var inactiveBikeCount = _bikeRep.GetQuery(b => b.StationId == e.Id && b.StatusId == 2).Count();

                return new StationResponse
                {
                    Id = e.Id,
                    StationName = e.StationName,
                    QuantityAvaiable = e.QuantityAvaiable,
                    NumOfSeats = e.NumOfSeats,
                    LocationName = e.LocationName,
                    Longitude = e.Longitude,
                    Latitude = e.Latitude,
                    StatusName = e.Status?.StatusName ?? "Unknown", // Kiểm tra null cho Status
                    StatusId = e.StatusId,
                    ActiveBikeCount = activeBikeCount,
                    InactiveBikeCount = inactiveBikeCount
                };
            }).ToList();

            if (string.IsNullOrEmpty(request.OrderBy) && string.IsNullOrEmpty(request.OrderByDesc))
            {
                stationResponses = stationResponses.OrderBy(e => e.Id).ToList();
            }
            else
            {
                stationResponses = PagingSorting.Sorting(request, stationResponses.AsQueryable()).ToList();
            }

            var total = stationResponses.Count;
            var pagedStations = stationResponses.Skip((request.PageIndex - 1) * request.PageSize).Take(request.PageSize).ToList();

            var result = new PagingResultSP<StationResponse>(pagedStations, total, request.PageIndex, request.PageSize);
            var i = (request.PageIndex - 1) * request.PageSize + 1;

            foreach (var item in result.Data)
            {
                item.Index = i++;
            }

            return result;
        }

        public async Task<PagingResultSP<StationResponse>> ListStation(ListStationUserCommand request)
        {
            double userLatitude = request.UserLatitude;
            double userLongitude = request.UserLongitude;

            var query = _stationRep.GetQuery().Include(e => e.Status);

            if (!string.IsNullOrEmpty(request.SearchTerm))
            {
                request.SearchTerm = request.SearchTerm.ToLower().Trim();
               // query = query.Where(e => e.StationName.ToLower().Contains(request.SearchTerm));
            }

            var stations = await query.ToListAsync();

            var stationResponses = stations.Select(e =>
            {
                var activeBikeCount = _bikeRep.GetQuery(b => b.StationId == e.Id && b.StatusId == 1).Count();
                var inactiveBikeCount = _bikeRep.GetQuery(b => b.StationId == e.Id && b.StatusId == 2).Count();
                var distance = _stationService.GetDistance(userLatitude, userLongitude, e.Latitude, e.Longitude);

                return new StationResponse
                {
                    Id = e.Id,
                    StationName = e.StationName,
                    QuantityAvaiable = e.QuantityAvaiable,
                    NumOfSeats = e.NumOfSeats,
                    LocationName = e.LocationName,
                    Longitude = e.Longitude,
                    Latitude = e.Latitude,
                    StatusName = e.Status?.StatusName ?? "Unknown", // Kiểm tra null cho Status
                    StatusId = e.StatusId,
                    ActiveBikeCount = activeBikeCount,
                    InactiveBikeCount = inactiveBikeCount,
                    Distance = distance
                };
            }).OrderBy(e => e.Distance).ToList();

            var total = stationResponses.Count;
            var pagedStations = stationResponses.Skip((request.PageIndex - 1) * request.PageSize).Take(request.PageSize).ToList();

            var result = new PagingResultSP<StationResponse>(pagedStations, total, request.PageIndex, request.PageSize);
            var i = (request.PageIndex - 1) * request.PageSize + 1;

            foreach (var item in result.Data)
            {
                item.Index = i++;
            }

            return result;
        }










    }
}
