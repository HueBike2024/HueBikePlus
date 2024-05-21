using Core.Exceptions;
using Core.Infrastructure.Handlers;
using Core.Interfaces.Database;
using Core.Properties;
using Core.SeedWork.Repository;
using GoogleApi.Entities.Interfaces;
using Infrastructure.AggregatesModel.Authen.AccountAggregate;
using Infrastructure.AggregatesModel.MasterData.BikeManagementAggregate.BikeAggregate;
using Infrastructure.AggregatesModel.MasterData.TripManagementAggregate.TicketAggregate;
using MasterData.Application.DTOs.Ticket;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterData.Application.Commands.TicketCommand
{
    public class ChangeBookInfoCommand : IRequest<TicketResponse>
    {
        public long TicketId { get; set; }
        public long BikeId { get; set; }
        public long CategoryTicketId { get; set; }
        public DateTime BookingDate { get; set; }
        public TimeSpan BookingTime { get; set; }
    }

    //public class ChangeBookInfoCommandHandler : BaseHandler, IRequestHandler<ChangeBookInfoCommand, TicketResponse>
    //{
    //    public readonly IRepository<Ticket> _tickRep;
    //    public readonly IRepository<Bike> _bikeRep;
    //    public readonly IRepository<CategoryTicket> _cateRep;
    //    public readonly IRepository<User> _userRep;
    //    public readonly IUnitOfWork _unitOfWork;

    //    public ChangeBookInfoCommandHandler(IRepository<Ticket> tickRep, IRepository<Bike> bikeRep, IRepository<User> userRep, IRepository<CategoryTicket> cateRep, IUnitOfWork unitOfWork)
    //    {
    //        _tickRep = tickRep;
    //        _bikeRep = bikeRep;
    //        _cateRep = cateRep;
    //        _userRep = userRep;
    //        _unitOfWork = unitOfWork;
    //    }
    //    public async Task<TicketResponse> Handle(ChangeBookInfoCommand request, CancellationToken cancellationToken)
    //    {
    //        var ticket = await _tickRep.FindOneAsync(e => e.Id == request.TicketId);
    //        var curCategoryTicket = await _cateRep.FindOneAsync(e => e.Id == ticket.CategoryTicketId);
    //        var user = await _userRep.FindOneAsync(e => e.Id == UserId);
    //        var resCategoryTicket = await _cateRep.FindOneAsync(e => e.Id == request.CategoryTicketId);
    //        var bike = await _bikeRep.FindOneAsync(e => e.Id == request.BikeId);
    //        var tickets = await _tickRep.GetQuery().Where(e => e.BikeId == request.BikeId && e.BookingDate == request.BookingDate).ToListAsync();

    //        if (ticket == null)
    //        {
    //            throw new BaseException(ErrorsMessage.MSG_NOT_EXIST, "Vé");
    //        }

    //        if (user == null)
    //        {
    //            throw new BaseException(ErrorsMessage.MSG_NOT_EXIST, "Người dùng");
    //        }

    //        if (bike == null)
    //        {
    //            throw new BaseException(ErrorsMessage.MSG_NOT_EXIST, "Xe");
    //        }

    //        if (resCategoryTicket == null)
    //        {
    //            throw new BaseException(ErrorsMessage.MSG_NOT_EXIST, "Loại Vé");
    //        }

    //        if (request.BookingDate == null)
    //        {
    //            throw new BaseException(ErrorsMessage.MSG_REQUIRED, "Ngày đặt");
    //        }

    //        if (request.BookingTime == null)
    //        {
    //            throw new BaseException(ErrorsMessage.MSG_REQUIRED, "Giờ đặt");
    //        }

    //        if (request.BookingDate < DateTime.Now.Date)
    //        {
    //            throw new BaseException("Ngày đặt không đúng!");
    //        }
    //        request.BookingDate = request.BookingDate.Add(request.BookingTime);
    //        if (request.BookingDate.Date == DateTime.Now.Date && request.BookingDate.TimeOfDay < DateTime.Now.TimeOfDay)
    //        {
    //            throw new BaseException("Giờ đặt không đúng");
    //        }

    //        if (ticket.CategoryTicketId != request.CategoryTicketId)
    //        {
    //            user.Point = user.Point + curCategoryTicket.Price;
    //        }

    //        if (user.Point < resCategoryTicket.Price)
    //        {
    //            throw new BaseException("Bạn không đủ điểm để mua loại vé này!");
    //        }

    //        if (tickets != null)
    //        {
    //            foreach (var item in tickets)
    //            {
    //                var cateTicket = await _cateRep.FindOneAsync(e => e.Id == item.CategoryTicketId);
    //                TimeSpan timeActive = item.BookingTime + cateTicket.UserTime;
    //                var isBooked = await _tickRep.GetAny(e => e.BikeId == request.BikeId && e.BookingDate == request.BookingDate && e.BookingTime <= request.BookingTime && request.BookingTime <= timeActive);

    //                if (isBooked)
    //                {
    //                    throw new BaseException("Xe đã được đặt trước!");
    //                }
    //            }
    //        }

    //        ticket.BikeId = request.BikeId;
    //        ticket.CategoryTicketId = request.CategoryTicketId;
    //        ticket.BookingDate = request.BookingDate;
    //        ticket.BookingTime = request.BookingTime;
    //        ticket.UpdatedDate = DateTime.UtcNow;

    //        user.Point -= resCategoryTicket.Price;
            
    //        _tickRep.Update(ticket);
    //        _userRep.Update(user);

    //        await _unitOfWork.SaveChangesAsync();

    //        var ticketResponse = new TicketResponse
    //        {
    //            TicketId = ticket.Id,
    //            TicketName = ticket.TicketName,
    //            PathQr = ticket.PathQr,
    //            BookingDate = ticket.BookingDate,
    //            BookingTime = ticket.BookingTime,
    //            CategoryTicketName = resCategoryTicket.CategoryTicketName,
    //            UserFullName = user.FullName,
    //            BikeName = bike.BikeName,
    //        };

    //        return ticketResponse;
    //    }
    //}
}
