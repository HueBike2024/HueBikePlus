using Core.Exceptions;
using Core.Interfaces.Database;
using Core.SeedWork.Repository;
using GoogleApi.Entities.Interfaces;
using Infrastructure.AggregatesModel.Authen;
using Infrastructure.AggregatesModel.Authen.AccountAggregate;
using Infrastructure.AggregatesModel.MasterData.BikeManagementAggregate.BikeAggregate;
using Infrastructure.AggregatesModel.MasterData.BikeManagementAggregate.StationAggregate;
using Infrastructure.AggregatesModel.MasterData.TripManagementAggregate.TicketAggregate;
using MasterData.Application.Commands.StatusCommand;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterData.Application.Commands.CategoryTicketCommand
{
    public class DeleteCategoryTicketCommmand : IRequest<bool>
    {
        public long Id { get; set; }    
    }

    public class DeleteCategoryTicketCommmandHandler : IRequestHandler<DeleteCategoryTicketCommmand, bool>
    {
        public readonly IRepository<CategoryTicket> _cateRep;
        public readonly IRepository<Ticket> _ticketRep;
        public readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<AuthenMedia> _media;

        public DeleteCategoryTicketCommmandHandler(IRepository<CategoryTicket> cateRep, IUnitOfWork unitOfWork, IRepository<AuthenMedia> media, IRepository<Ticket> ticketRep)
        {
            _cateRep = cateRep;
            _ticketRep = ticketRep;
            _unitOfWork = unitOfWork;
            _media = media;
        }
        public async Task<bool> Handle(DeleteCategoryTicketCommmand request, CancellationToken cancellationToken)
        {
            var categoryTicket = await _cateRep.FindOneAsync(e => e.Id == request.Id);
            if (categoryTicket == null)
            {
                throw new BaseException("Không tìm thấy loại vé cần xóa!");
            }

            var isInTicket = await _ticketRep.GetAny(e => e.CategoryTicketId == request.Id);

            if (isInTicket)
            {
                throw new BaseException("Loại vé đang được sử dụng, không thể xóa!");
            }

            _cateRep.Remove(categoryTicket);

            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}
