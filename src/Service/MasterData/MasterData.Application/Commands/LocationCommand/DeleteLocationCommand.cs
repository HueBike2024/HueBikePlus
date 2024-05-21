using Core.Exceptions;
using Core.Interfaces.Database;
using Core.Properties;
using Core.SeedWork.Repository;
using GoogleApi.Entities.Interfaces;
using GoogleApi.Entities.Maps.Common;
using Infrastructure.AggregatesModel.MasterData.BikeManagementAggregate.BikeAggregate;
using Infrastructure.AggregatesModel.MasterData.BikeManagementAggregate.MapLocationAggregate;
using Infrastructure.AggregatesModel.MasterData.BikeManagementAggregate.StationAggregate;
using MasterData.Application.DTOs.Location;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterData.Application.Commands.LocationCommand
{
    public class DeleteLocationCommand : IRequest<bool>
    {
        public long Id { get; set; }
    }

    public class DeleteLocationCommandHandler : IRequestHandler<DeleteLocationCommand, bool>
    {
        public readonly IRepository<MapLocation> _locationRep;
        public readonly IRepository<Bike> _bikeRep;
        public readonly IRepository<Station> _stationRep;
        public readonly IUnitOfWork _unitOfWork;

        public DeleteLocationCommandHandler(IRepository<MapLocation> locationRep, IUnitOfWork unitOfWork, IRepository<Bike> bikeRep, IRepository<Station> stationRep)
        {
            _locationRep = locationRep;
            _unitOfWork = unitOfWork;
            _bikeRep = bikeRep;
            _stationRep = stationRep;
        }
        public async Task<bool> Handle(DeleteLocationCommand request, CancellationToken cancellationToken)
        {
            var location = await _locationRep.FindOneAsync(e => e.Id == request.Id);
            if (location == null)
            {
                throw new BaseException("Không tìm thấy vị trí!");
            }

            //var station = await _stationRep.FindOneAsync(e => e.LocationId == request.Id);
            //if (station != null)
            //{
            //    throw new BaseException("Không thể xóa vị trí, vị trí này đang được sử dụng!");
            //}

            _locationRep.Remove(location);

            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}
