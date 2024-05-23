using Core.Exceptions;
using Core.Infrastructure.Handlers;
using Core.Properties;
using Core.SeedWork;
using Core.SeedWork.Repository;
using Infrastructure.AggregatesModel.Authen.AccountAggregate;
using Infrastructure.AggregatesModel.MasterData.BikeManagementAggregate.BikeAggregate;
using Infrastructure.AggregatesModel.MasterData.BikeManagementAggregate.StationAggregate;
using Infrastructure.AggregatesModel.MasterData.StatusAggregate;
using Infrastructure.AggregatesModel.MasterData.TripManagementAggregate.TicketAggregate;
using Infrastructure.AggregatesModel.MasterData.TripManagementAggregate.TripAggregate;
using MasterData.Application.Commands.TripCommand;
using MasterData.Application.Commands.UnitCommand;
using MasterData.Application.DTOs.Notification;
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

        /// <summary>
        /// Danh sách chuyến đi đang diễn ra của khách hàng
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<PagingResultSP<UserTripResponse>> ListOnTripUserAsync(ListUserTripCommand request);

        // <summary>
        /// Danh sách lich sử chuyến đi của khách hàng
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<PagingResultSP<ListTripEndedResponse>> ListEndedTripUserAsync(ListUserTripCommand request);

        // <summary>
        /// Danh sách tất cả chuyến đi
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<PagingResultSP<ListTripResponse>> ListTripAsync(ListTripCommand request);
    }

    public class TripQuery : BaseHandler, ITripQuery
    {
        private readonly IRepository<Bike> _bikeRep;
        private readonly IRepository<Trip> _tripRep;
        private readonly IRepository<Ticket> _ticketRep;
        private readonly IRepository<CategoryTicket> _cateRep;
        private readonly IRepository<Station> _stationRep;
        private readonly IRepository<Status> _statusRep;
        private readonly IRepository<User> _userRep;

        public TripQuery(IRepository<Ticket> ticketRep, IRepository<Bike> bikeRep, IRepository<Trip> tripRep, IRepository<Station> stationRep, IRepository<Status> statusRep, IRepository<User> userRep)
        {
            _ticketRep = ticketRep;
            _bikeRep = bikeRep;
            _tripRep = tripRep;
            _stationRep = stationRep;
            _statusRep = statusRep;
            _userRep = userRep;
        }
        public async Task<GetBikeInfoForTripResponse> GetBikeInfoAsync(GetBikeInfoForTripCommand request)
        {
            var bike = await _bikeRep.FindOneAsync(e => e.Id == request.Id);
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
                BikeCode = bike.BikeCode,
                StationName = station.StationName,
                Location = $"{station.Longitude} - {station.Latitude}"
            };
        }

        public async Task<PagingResultSP<ListTripEndedResponse>> ListEndedTripUserAsync(ListUserTripCommand request)
        {
            var listTripResponse = from Trip in _tripRep.GetQuery()
                                   join Ticket in _ticketRep.GetQuery() on Trip.TicketId equals Ticket.Id
                                   join CategoryTicket in _cateRep.GetQuery() on Ticket.CategoryTicketId equals CategoryTicket.Id
                                   join Bike in _bikeRep.GetQuery() on Ticket.BikeId equals Bike.Id
                                   join User in _userRep.GetQuery() on Ticket.UserId equals User.Id
                                   join StartStation in _stationRep.GetQuery() on Trip.StartStationId equals StartStation.Id
                                   join EndStation in _stationRep.GetQuery() on Trip.EndStationId equals EndStation.Id
                                   where Trip.IsEnd == true && User.Id == User.Id
                                   select new ListTripEndedResponse
                                   {
                                       BikeId = Bike.Id,
                                       StartTime = Trip.StartDate,
                                       EndTime = Trip.EndDate,
                                       StartStation = StartStation.StationName,
                                       EndStation = EndStation.StationName,
                                       TripStatus = Trip.IsDebt == true ? "Nợ cước" : "Hoàn thành",
                                       MinutesTraveled = Trip.MinutesTraveled,
                                       ExcessMinutes = Trip.ExcessMinutes,
                                       CategoryTicketName = CategoryTicket.CategoryTicketName,
                                   };

            if (string.IsNullOrEmpty(request.OrderBy) && string.IsNullOrEmpty(request.OrderByDesc))
            {
                listTripResponse = listTripResponse.OrderByDescending(e => e.EndTime);
            }
            else
            {
                listTripResponse = PagingSorting.Sorting(request, listTripResponse);
            }
            var pageIndex = request.PageSize * (request.PageIndex - 1);

            var response = await PaginatedList<ListTripEndedResponse>.CreateAsync(listTripResponse, pageIndex, request.PageSize);

            var result = new PagingResultSP<ListTripEndedResponse>(response, response.Total, request.PageIndex, request.PageSize);
            var i = pageIndex + 1;

            foreach (var item in result.Data)
            {
                item.Index = i++;
            }

            return result;
        }

        public async Task<PagingResultSP<UserTripResponse>> ListOnTripUserAsync(ListUserTripCommand request)
        {
            var listTripResponse = from Trip in _tripRep.GetQuery()
                                               join Ticket in _ticketRep.GetQuery() on Trip.TicketId equals Ticket.Id
                                               join CategoryTicket in _cateRep.GetQuery() on Ticket.CategoryTicketId equals CategoryTicket.Id
                                               join Bike in _bikeRep.GetQuery() on Ticket.BikeId equals Bike.Id
                                               join User in _userRep.GetQuery() on Ticket.UserId equals User.Id
                                               where ((Trip.IsEnd == false && Trip.IsDebt == false) || (Trip.IsEnd == false && Trip.IsDebt != false)) && User.Id == User.Id
                                               select new UserTripResponse
                                               {
                                                   BikeId = Bike.Id,
                                                   BikePower = Bike.Power,
                                                   TripStatus = Trip.IsDebt == true ? "Nợ cước" : "Đang hoạt động",
                                                   MinutesTraveled = Trip.MinutesTraveled,
                                                   ExcessMinutes = Trip.ExcessMinutes,
                                                   TripPrice = Trip.TripPrice,
                                                   CategoryTicketName = CategoryTicket.CategoryTicketName,
                                               };

            if (string.IsNullOrEmpty(request.OrderBy) && string.IsNullOrEmpty(request.OrderByDesc))
            {
                listTripResponse = listTripResponse.OrderBy(e => e.Index);
            }
            else
            {
                listTripResponse = PagingSorting.Sorting(request, listTripResponse);
            }
            var pageIndex = request.PageSize * (request.PageIndex - 1);

            var response = await PaginatedList<UserTripResponse>.CreateAsync(listTripResponse, pageIndex, request.PageSize);

            var result = new PagingResultSP<UserTripResponse>(response, response.Total, request.PageIndex, request.PageSize);
            var i = pageIndex + 1;

            foreach (var item in result.Data)
            {
                item.Index = i++;
            }

            return result;
        }

        public async Task<PagingResultSP<ListTripResponse>> ListTripAsync(ListTripCommand request)
        {
            var listTripResponse = from Trip in _tripRep.GetQuery()
                                   join Ticket in _ticketRep.GetQuery() on Trip.TicketId equals Ticket.Id
                                   join CategoryTicket in _cateRep.GetQuery() on Ticket.CategoryTicketId equals CategoryTicket.Id
                                   join Bike in _bikeRep.GetQuery() on Ticket.BikeId equals Bike.Id
                                   join User in _userRep.GetQuery() on Ticket.UserId equals User.Id
                                   join StartStation in _stationRep.GetQuery() on Trip.StartStationId equals StartStation.Id
                                   select new ListTripResponse
                                   {
                                       BikeCode = Bike.BikeCode,
                                       StatTime = Trip.StartDate,
                                       isEnd = Trip.IsEnd,
                                       isDebt = Trip.IsDebt,
                                       StartStation = StartStation.StationName,
                                       MinutesTraveled = Trip.MinutesTraveled,
                                       TripPrice = Trip.TripPrice,
                                   };

            if (!string.IsNullOrEmpty(request.SearchTerm))
            {
                request.SearchTerm = request.SearchTerm.ToLower().Trim();
                listTripResponse = listTripResponse.Where(e =>
                    e.BikeCode.ToLower().Contains(request.SearchTerm) 
                );
            }

            if (request.IsEnd == true && request.IsDebt == false)
            {
                listTripResponse = listTripResponse.Where(e =>
                    e.isEnd == true && e.isDebt == false
                );
            }

            if (request.IsEnd == false && request.IsDebt == true)
            {
                listTripResponse = listTripResponse.Where(e =>
                    e.isEnd == false && e.isDebt == true
                );
            }

            if (request.IsEnd == true && request.IsDebt == true)
            {
                listTripResponse = listTripResponse.Where(e =>
                    e.isEnd == true && e.isDebt == true
                );
            }

            if (string.IsNullOrEmpty(request.OrderBy) && string.IsNullOrEmpty(request.OrderByDesc))
            {
                listTripResponse = listTripResponse.OrderByDescending(e => e.StatTime);
            }
            else
            {
                listTripResponse = PagingSorting.Sorting(request, listTripResponse);
            }
            var pageIndex = request.PageSize * (request.PageIndex - 1);

            var response = await PaginatedList<ListTripResponse>.CreateAsync(listTripResponse, pageIndex, request.PageSize);

            var result = new PagingResultSP<ListTripResponse>(response, response.Total, request.PageIndex, request.PageSize);
            var i = pageIndex + 1;

            foreach (var item in result.Data)
            {
                item.Index = i++;
            }

            return result;
        }
    }
}
