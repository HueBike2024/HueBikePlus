using Core.Exceptions;
using Core.Infrastructure.Handlers;
using Core.Interfaces.Database;
using Core.Properties;
using Core.SeedWork.Repository;
using GoogleApi.Entities.Interfaces;
using Infrastructure.AggregatesModel.Authen.AccountAggregate;
using Infrastructure.AggregatesModel.MasterData.BikeManagementAggregate.BikeAggregate;
using Infrastructure.AggregatesModel.MasterData.BikeManagementAggregate.StationAggregate;
using Infrastructure.AggregatesModel.MasterData.NotificationAggregate;
using Infrastructure.AggregatesModel.MasterData.StatusAggregate;
using Infrastructure.AggregatesModel.MasterData.TripManagementAggregate.TicketAggregate;
using Infrastructure.AggregatesModel.MasterData.TripManagementAggregate.TripAggregate;
using Infrastructure.AggregatesModel.MasterData.UserAggregate;
using MasterData.Application.Commands.TicketCommand;
using MasterData.Application.DTOs.Trip;
using MasterData.Application.Services.GoogleMaps;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterData.Application.Commands.TripCommand
{
    public class CompleteTripCommand : IRequest<TripResponse>
    {
        public long TripId { get; set; }
        public double BikeLat { get; set; }
        public double BikeLng { get; set;}
    }

    public class CompleteTripCommandHandler : BaseHandler, IRequestHandler<CompleteTripCommand, TripResponse>
    {
        private readonly IMediator _mediator;
        public readonly IRepository<Ticket> _tickRep;
        public readonly IRepository<Trip> _tripRep;
        public readonly IRepository<Transaction> _tranRep;
        public readonly IRepository<Bike> _bikeRep;
        public readonly IRepository<BikeLock> _lockRep;
        public readonly IRepository<Station> _stationRep;
        public readonly IRepository<Status> _statusRep;
        public readonly IRepository<CategoryTicket> _cateRep;
        public readonly IRepository<Notification> _notiRep;
        public readonly IRepository<UserNotification> _uNotiRep;
        public readonly IRepository<User> _userRep;
        public readonly IUnitOfWork _unitOfWork;
        public readonly IMapService _mapService;

        public CompleteTripCommandHandler(IRepository<UserNotification> uNotiRep, IRepository<Notification> notiRep, IMapService mapService, IMediator mediator, IRepository<Ticket> tickRep, IRepository<Bike> bikeRep, IRepository<User> userRep, IRepository<CategoryTicket> cateRep, IUnitOfWork unitOfWork, IRepository<Transaction> tranRep, IRepository<Trip> tripRep, IRepository<BikeLock> lockRep, IRepository<Station> stationRep, IRepository<Status> statusRep)
        {
            _mediator = mediator;
            _tickRep = tickRep;
            _bikeRep = bikeRep;
            _cateRep = cateRep;
            _userRep = userRep;
            _unitOfWork = unitOfWork;
            _tranRep = tranRep;
            _tripRep = tripRep;
            _lockRep = lockRep;
            _stationRep = stationRep;
            _statusRep = statusRep;
            _mapService = mapService;
            _notiRep = notiRep;
            _uNotiRep = uNotiRep;
        }
        public async Task<TripResponse> Handle(CompleteTripCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRep.FindOneAsync(e => e.Id == UserId);
            var trip = await _tripRep.FindOneAsync(e => e.Id == request.TripId);
            var ticket = await _tickRep.FindOneAsync(e => e.Id == trip.TicketId);
            var categoryTicket = await _cateRep.FindOneAsync(e => e.Id == ticket.CategoryTicketId);
            var stationStart = await _stationRep.FindOneAsync(e => e.Id == trip.StartStationId);

            var stations = await _stationRep.GetQuery().ToListAsync();

            if (user == null)
            {
                throw new BaseException(ErrorsMessage.MSG_NOT_EXIST, " Người dùng");
            }

            if (trip == null)
            {
                throw new BaseException(ErrorsMessage.MSG_NOT_EXIST, "Chuyến đi");
            }

            trip.IsEnd = true;
            trip.EndDate = DateTime.Now;
            foreach( var station in stations)
            {
                if(_mapService.AreCoordinatesEqual(request.BikeLat, request.BikeLng, station.Latitude, station.Longitude))
                {
                    trip.EndStationId = station.Id;
                }
                else
                {
                    throw new BaseException("Hãy đặt xe đúng vào vị trí của trạm!");
                }
            }

            var stationEnd = await _stationRep.FindOneAsync(e => e.Id == trip.EndStationId);

            stationStart.QuantityAvaiable -= 1;
            stationEnd.QuantityAvaiable += 1;

            var status = await _statusRep.FindOneAsync(e => e.StatusName.Trim().ToLower() == "Chưa sử dụng".Trim().ToLower());
            var bike = await _bikeRep.FindOneAsync(e => e.Id == ticket.BikeId);

            bike.StatusId = status.Id;

            _tripRep.Update(trip);
            _stationRep.Update(stationEnd);
            _stationRep.Update(stationStart); 
            _statusRep.Update(status);
            _bikeRep.Update(bike);

            await _unitOfWork.SaveChangesAsync();

            var tripResponse = new TripResponse
            {
                TripId = trip.Id,
                BikeId = bike.Id,
                StartStation = stationStart.StationName,
                EndStation = stationEnd.StationName,
                TripStatus = trip.IsDebt == true ? "Nợ cước" : "Hoàn thành",
                MinutesTraveled = trip.MinutesTraveled,
                ExcessMinutes = trip.ExcessMinutes,
                TripPrice = trip.TripPrice,
                CategoryTicketName = categoryTicket.CategoryTicketName,
            };

            return tripResponse;
        }
    }
}
