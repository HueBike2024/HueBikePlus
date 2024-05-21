using Core.Exceptions;
using Core.Interfaces.Database;
using Core.Properties;
using Core.SeedWork.Repository;
using Infrastructure.AggregatesModel.MasterData.BikeManagementAggregate.MapLocationAggregate;
using Infrastructure.AggregatesModel.MasterData.StatusAggregate;
using MasterData.Application.Commands.StatusCommand;
using MasterData.Application.DTOs.Location;
using MasterData.Application.DTOs.Status;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterData.Application.Commands.LocationCommand
{
    public class CreateLocationCommand : IRequest<LocationResponse>
    {
        public string LocationName { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
    }

    public class CreateLocationCommandHandler : IRequestHandler<CreateLocationCommand, LocationResponse>
    {
        public readonly IRepository<MapLocation> _locationRep;
        public readonly IUnitOfWork _unitOfWork;

        public CreateLocationCommandHandler(IRepository<MapLocation> locationRep, IUnitOfWork unitOfWork)
        {
            _locationRep = locationRep;
            _unitOfWork = unitOfWork;
        }
        public async Task<LocationResponse> Handle(CreateLocationCommand request, CancellationToken cancellationToken)
        {
            var isEXLocation = await _locationRep.GetAny(e => e.LocationName == request.LocationName || (e.Longitude == request.Longitude && e.Latitude == request.Latitude));

            if (isEXLocation)
            {
                throw new BaseException("Đã có vị trí này hoặc tên vị trí không hợp lệ");
            }

            if (string.IsNullOrEmpty(request.LocationName))
            {
                throw new BaseException(ErrorsMessage.MSG_NOT_EXIST, "Tên vị trí");
            }

            //if (string.IsNullOrEmpty(request.Longitude))
            //{
            //    throw new BaseException(ErrorsMessage.MSG_NOT_EXIST, "Kinh độ");
            //}

            //if (string.IsNullOrEmpty(request.Latitude))
            //{
            //    throw new BaseException(ErrorsMessage.MSG_NOT_EXIST, "Vĩ độ");
            //}

            var location = new MapLocation(request.LocationName, request.Longitude, request.Latitude);

            _locationRep.Add(location);

            await _unitOfWork.SaveChangesAsync();


            var result = new LocationResponse
            {
                Id = location.Id,
                LocationName = request.LocationName,
                Logitude = request.Longitude,
                Latitude = request.Latitude,
                CreatedDate = location.CreatedDate
            };

            return result;
        }
    }
}
