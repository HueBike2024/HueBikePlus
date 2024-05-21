using Core.Exceptions;
using Core.Interfaces.Database;
using Core.Properties;
using Core.SeedWork.Repository;
using Infrastructure.AggregatesModel.Authen.AccountAggregate;
using Infrastructure.AggregatesModel.MasterData.BikeManagementAggregate.BikeAggregate;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterData.Application.Commands.BikeLockCommand
{
    public class DeleteLockCommand : IRequest<bool>
    {
        public long Id { get; set; }
    }

    public class DeleteLockCommandHandler : IRequestHandler<DeleteLockCommand, bool>
    {

        private readonly IRepository<BikeLock> _lockRep;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteLockCommandHandler(IRepository<BikeLock> lockRep, IUnitOfWork unitOfWork)
        {
            _lockRep = lockRep;
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(DeleteLockCommand request, CancellationToken cancellationToken)
        {
            var bikelock = await _lockRep.FindOneAsync(e => e.Id == request.Id);

            if (bikelock == null)
            {
                throw new BaseException(ErrorsMessage.MSG_NOT_EXIST, "Khóa");
            }

            

            BikeLock.DeleteLock(ref bikelock);


            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}
