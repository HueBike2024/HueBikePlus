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

namespace MasterData.Application.Commands.StationCommand
{
    public class DeleteStationCommand : IRequest<bool>
    {
        public long Id { get; set; }
    }

    public class DeleteStationCommandHandler : IRequestHandler<DeleteStationCommand, bool>
    {

        private readonly IRepository<Station> _stationRep;
        private readonly IRepository<Bike> _bikeRep;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteStationCommandHandler(IRepository<Bike> bikeRep,IRepository<Station> stationRep, IUnitOfWork unitOfWork)
        {
            _bikeRep = bikeRep;
            _stationRep = stationRep;
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(DeleteStationCommand request, CancellationToken cancellationToken)
        {
            var station = await _stationRep.FindOneAsync(e => e.Id == request.Id);

            if (station == null)
            {
                throw new BaseException(ErrorsMessage.MSG_NOT_EXIST, "Station");
            }

            // 
            var bikeCount = await _bikeRep.CountAsync(b => b.StationId == request.Id);
            if (bikeCount > 0)
            {
                throw new BaseException("Không thể xóa trạm khi xe đang còn.");
            }

            _stationRep.Remove(station);

            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}
