using Core.Exceptions;
using Core.Interfaces.Database;
using Core.Properties;
using Core.SeedWork.Repository;
using Infrastructure.AggregatesModel.MasterData.BikeManagementAggregate.StationAggregate;
using MediatR;
using Microsoft.AspNetCore.Http;
using Status = Infrastructure.AggregatesModel.MasterData.StatusAggregate.Status;

namespace MasterData.Application.Commands.StationCommand
{
    public class CreateStationCommand : IRequest<bool>
    {

        public long? StationId { get; set; }
        public string StationName { get; set; }
        // public int QuantityAvaiable { get; set; }
        public int NumOfSeats { get; set; }
        public string LocationName { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public long StatusId { get; set; }


    }

    public class CreateStationCommandHandler : IRequestHandler<CreateStationCommand, bool>

    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public readonly IRepository<Station> _stationRep;
        public readonly IRepository<Status> _statusRep;

        public readonly IUnitOfWork _unitOfWork;

        public CreateStationCommandHandler(IHttpContextAccessor httpContextAccessor, IRepository<Status> statusRep, IRepository<Station> stationRep, IUnitOfWork unitOfWork)
        {
            _stationRep = stationRep;
            _unitOfWork = unitOfWork;

            _httpContextAccessor = httpContextAccessor;
            _statusRep = statusRep;
        }
        public async Task<bool> Handle(CreateStationCommand request, CancellationToken cancellationToken)
        {

            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext.Request.Method == HttpMethods.Post && (request.StationId == null || request.StationId == 0))
            {

                var isStation = await _stationRep.GetAny(e => e.StationName.Trim().ToLower() == request.StationName.Trim().ToLower());

                // kiểm tra điều kiện
                if (isStation)
                {
                    throw new BaseException(ErrorsMessage.MSG_EXIST, "Tên Trạm");
                }


                var station = new Station(request.StationName, request.LocationName,request.Longitude,request.Latitude, request.NumOfSeats, request.StatusId);

                _stationRep.Add(station);

                await _unitOfWork.SaveChangesAsync();

                return true;



            }
            else
            {
                var station = await _stationRep.FindOneAsync(e => e.Id == request.StationId);
                if (station == null)
                {
                    throw new BaseException("Không tìm thấy trạm!");
                }

                //var isExis = await _stationRep.GetAny(e => e.StationName == request.StationName);

                //if (isExis)
                //{
                //    throw new BaseException(ErrorsMessage.MSG_EXIST, "Tên trạm");
                //}

                if (string.IsNullOrEmpty(request.StationName))
                {
                    throw new BaseException(ErrorsMessage.MSG_NOT_EXIST, "Vui lòng không bỏ trống tên trạm");
                }







                station.StationName = request.StationName;               
                station.NumOfSeats = request.NumOfSeats;
                station.LocationName = request.LocationName;
               
                station.Longitude = request.Longitude;
                station.Latitude = request.Latitude;

                station.StatusId = request.StatusId;


                station.UpdatedDate = DateTime.UtcNow;

                _stationRep.Update(station);

                await _unitOfWork.SaveChangesAsync();
                return true;
            }

        }
    }
}
