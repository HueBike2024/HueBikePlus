using Core.Exceptions;
using Core.Infrastructure.Handlers;
using Core.Interfaces.Database;
using Core.Properties;
using Core.SeedWork.Repository;
using Infrastructure.AggregatesModel.Authen.AccountAggregate;
using Infrastructure.AggregatesModel.MasterData.BikeManagementAggregate.BikeAggregate;
using Infrastructure.AggregatesModel.MasterData.TripManagementAggregate.TicketAggregate;
using Infrastructure.AggregatesModel.MasterData.UserAggregate;
using Infrastructure.Services;
using MasterData.Application.DTOs.Ticket;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterData.Application.Commands.TicketCommand
{
    public class DeleteTicketCommand : IRequest<bool>
    {
        // Chức năng hủy vé của khách hàng hoặc admin có thể thực hiện hủy
        public long TicketId { get; set;}

    }

    public class DeleteTicketCommandHandler : BaseHandler, IRequestHandler<DeleteTicketCommand, bool>
    {
        public readonly IRepository<Ticket> _tickRep;
        public readonly IRepository<Transaction> _tranRep;
        public readonly IRepository<Bike> _bikeRep;
        public readonly IRepository<CategoryTicket> _cateRep;
        public readonly IRepository<User> _userRep;
        public readonly IUnitOfWork _unitOfWork;
        private readonly IRandomService _randomService;

        public DeleteTicketCommandHandler(IRepository<Ticket> tickRep, IRandomService randomService, IRepository<Bike> bikeRep, IRepository<User> userRep, IRepository<CategoryTicket> cateRep, IUnitOfWork unitOfWork, IRepository<Transaction> tranRep)
        {
            _tickRep = tickRep;
            _bikeRep = bikeRep;
            _cateRep = cateRep;
            _userRep = userRep;
            _unitOfWork = unitOfWork;
            _tranRep = tranRep;
            _randomService = randomService;
        }
        public async Task<bool> Handle(DeleteTicketCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRep.FindOneAsync(e => e.Id == UserId);
            var ticket = await _tickRep.FindOneAsync(e => e.Id == request.TicketId);
            var categoryTicket = await _cateRep.FindOneAsync(e => e.Id == ticket.CategoryTicketId);

            if (user == null)
            {
                throw new BaseException(ErrorsMessage.MSG_NOT_EXIST, "Người dùng");
            }

            if (ticket == null)
            {
                throw new BaseException(ErrorsMessage.MSG_NOT_EXIST, "Vé");
            }

            if (user.Id != ticket.UserId && user.IsSuperAdmin == false)
            {
                throw new BaseException("Bạn không có quyền hủy vé của một người khác!");
            }

            if (ticket.BookingDate <= DateTime.Now)
            {
                throw new BaseException("Bạn không thể hủy một vé đã đến giờ đặt!");
            }

            user.Point += categoryTicket.Price;

            ticket.IsDeleted = true;

            _tickRep.Update(ticket);
            _userRep.Update(user);

            var transaction = new Transaction("Hủy vé", _randomService.GenerateRandomString(16), + categoryTicket.Price, user.Id, true);
            _tranRep.Add(transaction);

            await _unitOfWork.SaveChangesAsync();


            return true;
        }
    }
}
