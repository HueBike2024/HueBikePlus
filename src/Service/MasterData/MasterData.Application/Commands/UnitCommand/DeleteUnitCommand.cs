using Core.Interfaces.Database;
using Core.SeedWork.Repository;
using Core.Properties;
using Core.Exceptions;
using MediatR;
using Unit = Infrastructure.AggregatesModel.MasterData.UnitAggregate.Unit;

namespace MasterData.Application.Commands.UnitCommand
{
    public class DeleteUnitCommand: IRequest<bool>
    {
        public long Id { get; set; }
    }
    public class DeleteUnitCommandHandler: IRequestHandler<DeleteUnitCommand, bool> {

        private readonly IRepository<Unit> _unitRep;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteUnitCommandHandler(IRepository<Unit> unitRep, IUnitOfWork unitOfWork)
        {
            _unitRep = unitRep;
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(DeleteUnitCommand request, CancellationToken cancellationToken)
        {
            var unit = await _unitRep.FindOneAsync(e => e.Id == request.Id);

            if (unit == null)
            {
                throw new BaseException(ErrorsMessage.MSG_NOT_EXIST, "Unit");
            }

            _unitRep.Remove(unit);

            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}
