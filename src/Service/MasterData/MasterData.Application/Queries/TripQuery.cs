using Core.Exceptions;
using Core.Infrastructure.Handlers;
using Core.Properties;
using Core.SeedWork.Repository;
using Infrastructure.AggregatesModel.Authen.AccountAggregate;
using Infrastructure.AggregatesModel.MasterData.BikeManagementAggregate.BikeAggregate;
using Infrastructure.AggregatesModel.MasterData.BikeManagementAggregate.StationAggregate;
using Infrastructure.AggregatesModel.MasterData.StatusAggregate;
using Infrastructure.AggregatesModel.MasterData.TripManagementAggregate.TripAggregate;
using MasterData.Application.Commands.TripCommand;
using MasterData.Application.Commands.UnitCommand;
using MasterData.Application.DTOs.Trip;
using MasterData.Application.DTOs.Unit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterData.Application.Queries
{
    public interface ITripQuery
    {
        // Todo: Lấy danh sách các chuyến đi tùy theo trạng thái( hoàn thành, đang đi, nợ cước), lấy danh sách chuyến đi của khách hàng & xem thông tin chuyến đi
        /// <summary>
        /// Lấy thông tin xe trả về để thực hiện chuyến đi
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<GetBikeInfoForTripResponse> GetBikeInfoAsync(GetBikeInfoForTripCommand request);
    }

    public class TripQuery : BaseHandler, ITripQuery
    {
        private readonly IRepository<Bike> _bikeRep;
        private readonly IRepository<Trip> _tripRep;
        private readonly IRepository<Station> _stationRep;
        private readonly IRepository<Status> _statusRep;
        private readonly IRepository<User> _userRep;

        public TripQuery(IRepository<Bike> bikeRep, IRepository<Trip> tripRep, IRepository<Station> stationRep, IRepository<Status> statusRep, IRepository<User> userRep)
        {
            _bikeRep = bikeRep;
            _tripRep = tripRep;
            _stationRep = stationRep;
            _statusRep = statusRep;
            _userRep = userRep;
        }
        public async Task<GetBikeInfoForTripResponse> GetBikeInfoAsync(GetBikeInfoForTripCommand request)
        {
            var bike = await _bikeRep.FindOneAsync(e => e.PathQr == request.PathQr);
            if (bike == null)
            {
                throw new BaseException(ErrorsMessage.MSG_NOT_EXIST, "Xe");
            }

            var station = await _stationRep.FindOneAsync(e => e.Id == bike.StationId);
            var status = await _statusRep.FindOneAsync(e => e.Id == bike.StatusId);
            if(!status.StatusName.Trim().ToLower().Contains("Chưa sử dụng".Trim().ToLower()))
            {
                throw new BaseException("Xe hiện tại không thể sử dụng!");
            }

            return new GetBikeInfoForTripResponse
            {
                Id = bike.Id,
                StationName = station.StationName,
                Location = $"{station.Longitude} - {station.Latitude}"
            };
        }
    }
}
