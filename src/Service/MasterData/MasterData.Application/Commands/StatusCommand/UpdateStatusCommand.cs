using Core.Exceptions;
using Core.Interfaces.Database;
using Core.Properties;
using Core.SeedWork.Repository;
using Infrastructure.AggregatesModel.MasterData.StatusAggregate;
using MasterData.Application.DTOs.Status;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterData.Application.Commands.StatusCommand
{
    public class UpdateStatusCommand : IRequest<StatusResponse>
    {
        public long Id { get; set; }
        public string Name { get; set; }
    }

    public class UpdateStatusCommandHandler : IRequestHandler<UpdateStatusCommand, StatusResponse>
    {
        public readonly IRepository<Status> _statusRep;
        public readonly IUnitOfWork _unitOfWork;

        public UpdateStatusCommandHandler(IRepository<Status> statusRep, IUnitOfWork unitOfWork)
        {
            _statusRep = statusRep;
            _unitOfWork = unitOfWork;
        }
        public async Task<StatusResponse> Handle(UpdateStatusCommand request, CancellationToken cancellationToken)
        {
            var status = await _statusRep.FindOneAsync(e => e.Id == request.Id);
            var isEXName = await _statusRep.GetAny(e => e.StatusName == request.Name);

            if (isEXName)
            {
                throw new BaseException(ErrorsMessage.MSG_EXIST, "Tên trạng thái");
            }

            if (string.IsNullOrEmpty(request.Name))
            {
                throw new BaseException("Vui lòng nhập tên trạng thái");
            }

            status.StatusName = request.Name;
            status.UpdatedDate = DateTime.UtcNow;

            _statusRep.Update(status);

            await _unitOfWork.SaveChangesAsync();

            var result = new StatusResponse
            {
                Id = status.Id,
                StatusName = status.StatusName,
                CreatedDate = status.CreatedDate,
                UpdateDate = status.UpdatedDate,
            };

            return result;
        }
    }
}
