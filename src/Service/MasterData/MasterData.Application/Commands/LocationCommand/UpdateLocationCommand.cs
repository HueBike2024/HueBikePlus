using Core.Exceptions;
using Core.Interfaces.Database;
using Core.Properties;
using Core.SeedWork.Repository;
using Infrastructure.AggregatesModel.MasterData.BikeManagementAggregate.MapLocationAggregate;
using MasterData.Application.DTOs.Location;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterData.Application.Commands.LocationCommand
{
    public class UpdateLocationCommand : IRequest<LocationResponse>
    {
        public long Id { get; set; }
        public string LocationName { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
    }

    public class UpdateLocationCommandHandler : IRequestHandler<UpdateLocationCommand, LocationResponse>
    {
        public readonly IRepository<MapLocation> _locationRep;
        public readonly IUnitOfWork _unitOfWork;

        public UpdateLocationCommandHandler(IRepository<MapLocation> locationRep, IUnitOfWork unitOfWork)
        {
            _locationRep = locationRep;
            _unitOfWork = unitOfWork;
        }
        public async Task<LocationResponse> Handle(UpdateLocationCommand request, CancellationToken cancellationToken)
        {
            var location = await _locationRep.FindOneAsync(e => e.Id == request.Id);
            if (location == null)
            {
                throw new BaseException("Không tìm thấy vị trí!");
            }

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

            location.LocationName = request.LocationName;
            location.Longitude = request.Longitude;
            location.Latitude = request.Latitude;
            location.UpdatedDate = DateTime.UtcNow;

            _locationRep.Update(location);

            await _unitOfWork.SaveChangesAsync();


            var result = new LocationResponse
            {
                Id = request.Id,
                LocationName = request.LocationName,
                Logitude = request.Longitude,
                Latitude = request.Latitude,
                CreatedDate = location.CreatedDate,
                UpdatedDate = location.UpdatedDate
            };

            return result;
        }
    }
}
