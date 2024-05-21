using Core.Exceptions;
using Core.Infrastructure.Handlers;
using Core.Interfaces.Database;
using Core.Properties;
using Core.SeedWork.Repository;
using Infrastructure.AggregatesModel.Authen.AccountAggregate;
using Infrastructure.AggregatesModel.MasterData.BikeManagementAggregate.BikeAggregate;
using Infrastructure.AggregatesModel.MasterData.BikeManagementAggregate.BikeStationAggregate;
using Infrastructure.AggregatesModel.MasterData.BikeManagementAggregate.StationAggregate;
using Infrastructure.AggregatesModel.MasterData.StatusAggregate;
using Infrastructure.AggregatesModel.MasterData.TripManagementAggregate.TicketAggregate;
using Infrastructure.AggregatesModel.MasterData.TripManagementAggregate.TripAggregate;
using Infrastructure.AggregatesModel.MasterData.UserAggregate;
using MasterData.Application.Commands.TicketCommand;
using MasterData.Application.DTOs.Ticket;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterData.Application.Commands.TripCommand
{
    public class NewTripCommand : IRequest<bool>
    {
        public long? TicketId { get; set; }
        public long? CategoryTicketId { get; set; }
        public long? BikeId { get; set; }
    }

    public class NewTripCommandHandler : BaseHandler, IRequestHandler<NewTripCommand, bool>
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
        public readonly IRepository<User> _userRep;
        public readonly IUnitOfWork _unitOfWork;

        public NewTripCommandHandler(IMediator mediator, IRepository<Ticket> tickRep, IRepository<Bike> bikeRep, IRepository<User> userRep, IRepository<CategoryTicket> cateRep, IUnitOfWork unitOfWork, IRepository<Transaction> tranRep, IRepository<Trip> tripRep, IRepository<BikeLock> lockRep, IRepository<Station> stationRep, IRepository<Status> statusRep)
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
        }
        public async Task<bool> Handle(NewTripCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRep.FindOneAsync(e => e.Id == UserId);
            var ticket = await _tickRep.FindOneAsync(e => e.Id == request.TicketId);
            var bike = await _bikeRep.FindOneAsync(e => e.Id == request.BikeId);
            var station = await _stationRep.FindOneAsync(e => e.Id == bike.StationId);
            var categoryTicket = await _cateRep.FindOneAsync(e => e.Id == request.CategoryTicketId);
            if (user == null)
            {
                throw new BaseException(ErrorsMessage.MSG_NOT_EXIST," Người dùng");
            }


            if (ticket != null)
            {
                if(ticket.BookingDate > DateTime.Now)
                {
                    throw new BaseException("Vé của bạn chưa tới thời gian sử dụng!");
                }
                if (DateTime.Now < ticket.ExpiryDate)
                {
                    throw new BaseException("Vé của bạn đã quá hạn, không thể sử dụng!");
                }

                if (bike == null)
                {
                    throw new BaseException("Xe không tồn tại hoặc không hợp lệ!");
                }

                if (bike != null && bike.Id != ticket.BikeId)
                {
                    throw new BaseException("Xe không đúng với vé bạn đã đặt!");
                }

                if (user.Point < 10000)
                {
                    throw new BaseException("Bạn cần phải có ít nhất 10.000 điểm để bắt đầu chuyến đi!");
                }

                var trip = new Trip(false, false, station.Id, ticket.Id);

                _tripRep.Add(trip);
                
                await _unitOfWork.SaveChangesAsync();

                return true;
            }
            else
            {
                if (categoryTicket == null)
                {
                    throw new BaseException("Vé không tồn tại hoặc không hợp lệ!");
                }

                if (bike == null)
                {
                    throw new BaseException("Xe không tồn tại hoặc không hợp lệ!");
                }

                var addTicketCommand = new PreBookTicketCommand
                {
                    TicketId = null,
                    UserPhone = null,
                    BikeId = bike.Id,
                    CategoryTicketId = categoryTicket.Id,
                    BookingDate = DateTime.Now,
                };

                var addTicketResponse = await _mediator.Send(addTicketCommand);

                if (addTicketResponse == null)
                {
                    throw new BaseException("Có lỗi khi thực hiện chuẩn bị cho chuyến đi!");
                }

                var trip = new Trip(false, false, station.Id, addTicketResponse.Id);

                _tripRep.Add(trip);

                await _unitOfWork.SaveChangesAsync();

                return true;
            }
        }
    }
}
