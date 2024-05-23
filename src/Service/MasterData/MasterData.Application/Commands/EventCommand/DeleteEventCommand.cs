using Core.Exceptions;
using Core.Interfaces.Database;
using Core.Properties;
using Core.SeedWork.Repository;
using Infrastructure.AggregatesModel.MasterData.EventAggregate;
using MediatR;

namespace MasterData.Application.Commands.EventCommand
{
    public class DeleteEventCommand : IRequest<bool>
    {
        public long Id { get; set; }
    }

    public class DeleteEventCommandHandler : IRequestHandler<DeleteEventCommand, bool>
    {

        private readonly IRepository<Event> _evtRep;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteEventCommandHandler(IRepository<Event> evtRep, IUnitOfWork unitOfWork)
        {
            _evtRep = evtRep;
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(DeleteEventCommand request, CancellationToken cancellationToken)
        {
            var evt = await _evtRep.FindOneAsync(e => e.Id == request.Id);

            if (evt == null)
            {
                throw new BaseException(ErrorsMessage.MSG_NOT_EXIST, "Event");
            }



            Event.DeleteEvent(ref evt);


            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}
