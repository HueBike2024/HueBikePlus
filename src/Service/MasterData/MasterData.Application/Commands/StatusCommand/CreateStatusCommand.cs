using Core.Exceptions;
using Core.Interfaces.Database;
using Core.Properties;
using Core.SeedWork.Repository;
using Infrastructure.AggregatesModel.MasterData.StatusAggregate;
using MasterData.Application.Commands.UnitCommand;
using MasterData.Application.DTOs.Status;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterData.Application.Commands.StatusCommand
{
    public class CreateStatusCommand : IRequest<StatusResponse>
    {
        public string Name { get; set; }
    }

    public class CreateStatusCommandHandler : IRequestHandler<CreateStatusCommand, StatusResponse>
    {
        public readonly IRepository<Status> _statusRep;
        public readonly IUnitOfWork _unitOfWork;

        public CreateStatusCommandHandler(IRepository<Status> statusRep, IUnitOfWork unitOfWork)
        {
            _statusRep = statusRep;
            _unitOfWork = unitOfWork;
        }
        public async Task<StatusResponse> Handle(CreateStatusCommand request, CancellationToken cancellationToken)
        {
            var isEXName = await _statusRep.GetAny(e => e.StatusName == request.Name);

            if (isEXName)
            {
                throw new BaseException(ErrorsMessage.MSG_EXIST, "Tên trạng thái");
            }

            if (string.IsNullOrEmpty(request.Name))
            {
                throw new BaseException(ErrorsMessage.MSG_NOT_EXIST, "Tên trạng thái");
            }

            var status = new Status(request.Name);

            _statusRep.Add(status);

            await _unitOfWork.SaveChangesAsync();


            var result = new StatusResponse
            {
                Id = status.Id,
                StatusName = status.StatusName,
                CreatedDate = status.CreatedDate,
            };

            return result;
        }
    }
}
