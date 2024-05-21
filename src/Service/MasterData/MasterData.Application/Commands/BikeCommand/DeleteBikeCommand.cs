using Core.Exceptions;
using Core.Interfaces.Database;
using Core.Properties;
using Core.SeedWork.Repository;
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
    public class DeleteBikeCommand : IRequest<bool>
    {
        public long Id { get; set; }
    }

    public class DeleteBikeCommandHandler : IRequestHandler<DeleteBikeCommand, bool>
    {

        private readonly IRepository<Bike> _bikeRep;
        private readonly IRepository<Station> _stationRep;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteBikeCommandHandler(IRepository<Station> stationRep, IRepository<Bike> bikeRep, IUnitOfWork unitOfWork)
        {
            _bikeRep = bikeRep;
            _stationRep = stationRep;
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(DeleteBikeCommand request, CancellationToken cancellationToken)
        {
            var bike = await _bikeRep.FindOneAsync(e => e.Id == request.Id);

            if (bike == null)
            {
                throw new BaseException(ErrorsMessage.MSG_NOT_EXIST, "Bike");
            }

            var station = await _stationRep.FindOneAsync(e => e.Id == bike.StationId);

            if (station != null)
            {
                station.QuantityAvaiable -= 1;
            }

            _bikeRep.Remove(bike);


            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}
