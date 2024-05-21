using Core.Exceptions;
using Core.Interfaces.Database;
using Core.Properties;
using Core.SeedWork.Repository;
using Infrastructure.AggregatesModel.Authen.AccountAggregate;
using Infrastructure.AggregatesModel.MasterData.BikeManagementAggregate.BikeAggregate;
using Infrastructure.AggregatesModel.MasterData.BikeManagementAggregate.StationAggregate;
using Infrastructure.AggregatesModel.MasterData.StatusAggregate;
using Infrastructure.AggregatesModel.MasterData.TripManagementAggregate.TripAggregate;
using MasterData.Application.DTOs.Status;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterData.Application.Commands.StatusCommand
{
    public class DeleteStatusCommand : IRequest<bool>
    {
        public long Id { get; set; }
    }

    public class DeleteStatusCommandHandler : IRequestHandler<DeleteStatusCommand, bool>
    {
        public readonly IRepository<Status> _statusRep;
        public readonly IRepository<User> _userRep;
        public readonly IRepository<Station> _stationRep;
        public readonly IRepository<Bike> _bikeRep;
        public readonly IUnitOfWork _unitOfWork;

        public DeleteStatusCommandHandler(IRepository<Status> statusRep, IUnitOfWork unitOfWork, IRepository<User> userRep, IRepository<Station> stationRep, IRepository<Bike> bikeRep)
        {
            _statusRep = statusRep;
            _unitOfWork = unitOfWork;
            _userRep = userRep;
            _stationRep = stationRep;
            _bikeRep = bikeRep;
        }
        public async Task<bool> Handle(DeleteStatusCommand request, CancellationToken cancellationToken)
        {
            if (request.Id == null)
            {
                throw new BaseException("Không tìm thấy trạng thái cần xóa!");
            }

            var status = await _statusRep.FindOneAsync(e => e.Id == request.Id);

            if (status == null)
            {
                throw new BaseException("Không tìm thấy trạng thái cần xóa!");
            }

            var isInBike = await _bikeRep.GetAny(e => e.StatusId == request.Id);
            var isInUser = await _userRep.GetAny(e => e.StatusId == request.Id);
            var isInStation = await _stationRep.GetAny(e => e.StatusId == request.Id);

            if (isInBike || isInUser || isInStation)
            {
                throw new BaseException("Trạng thái đang được sử dụng, không thể xóa!");
            }

            _statusRep.Remove(status);

            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}
