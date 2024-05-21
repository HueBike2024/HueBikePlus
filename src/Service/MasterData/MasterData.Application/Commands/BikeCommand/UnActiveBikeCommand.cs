using Core.Exceptions;
using Core.Interfaces.Database;
using Core.Properties;
using Core.SeedWork.Repository;
using GoogleApi.Entities.Interfaces;
using Infrastructure.AggregatesModel.MasterData.BikeManagementAggregate.BikeAggregate;
using Infrastructure.AggregatesModel.MasterData.BikeManagementAggregate.StationAggregate;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterData.Application.Commands.BikeCommand
{
    public class UnActiveBikeCommand : IRequest<bool>
    {
        public long BikeId { get; set; }
    }

    public class UnActiveBikeCommandHandler : IRequestHandler<UnActiveBikeCommand, bool>
    {

        private readonly IRepository<Bike> _bikeRep;
        private readonly IRepository<Station> _stationRep;
        private readonly IUnitOfWork _unitOfWork;

        public UnActiveBikeCommandHandler(IRepository<Station> stationRep, IRepository<Bike> bikeRep, IUnitOfWork unitOfWork)
        {
            _bikeRep = bikeRep;
            _stationRep = stationRep;
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(UnActiveBikeCommand request, CancellationToken cancellationToken)
        {
            var bike = await _bikeRep.FindOneAsync(e => e.Id == request.BikeId);

            if (bike == null)
            {
                throw new BaseException(ErrorsMessage.MSG_NOT_EXIST, "Bike");
            }

            if (bike.IsActive == false)
            {
                throw new BaseException("Xe đã được dừng kích hoạt trước đó!");
            }

            bike.IsActive = false;


            _bikeRep.Update(bike);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}
