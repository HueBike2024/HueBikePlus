using Core.Exceptions;
using Core.Interfaces.Database;
using Core.Properties;
using Core.SeedWork.Repository;
using MediatR;
using System.ComponentModel.DataAnnotations;
using Unit = Infrastructure.AggregatesModel.MasterData.UnitAggregate.Unit;

namespace MasterData.Application.Commands.UnitCommand
{
    public class UpdateUnitCommand: IRequest<long>
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        [EmailAddress(ErrorMessage = "Email incorrect formatted")] //Todo: sau này sẽ update mã hóa thông tin 
        public string Email { get; set; }
        [Phone(ErrorMessage = "Phone number incorrect formatted")] //Todo: sau này sẽ update mã hóa thông tin 
        public string PhoneNumber { get; set; }
        public string Fax { get; set; }

    }
    public class UpdateUnitCommandHandler: IRequestHandler<UpdateUnitCommand, long>
    {
        private readonly IRepository<Unit> _unitRep;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateUnitCommandHandler(IRepository<Unit> unitRep, IUnitOfWork unitOfWork)
        {
            _unitRep = unitRep;
            _unitOfWork = unitOfWork;
        }
        public async Task<long> Handle(UpdateUnitCommand request, CancellationToken cancellationToken)
        {
            var unit = await _unitRep.FindOneAsync(e => e.Id == request.Id);

            if (unit == null)
            {
                throw new BaseException(ErrorsMessage.MSG_NOT_EXIST, "Unit");
            }

            var isExistName = await _unitRep.GetAny(e => e.Name.Trim().ToLower() == request.Name.Trim().ToLower() && e.Id != request.Id);

            if (isExistName)
            {
                throw new BaseException(ErrorsMessage.MSG_EXIST, "Name");
            }

            var isExistCode = await _unitRep.GetAny(e => e.Code.Trim().ToLower() == request.Code.Trim().ToLower() && e.Id != request.Id);

            if (isExistCode)
            {
                throw new BaseException(ErrorsMessage.MSG_EXIST, "Code");
            }

            var isExistEmail = await _unitRep.GetAny(e => e.Email == request.Email && e.Id != request.Id);

            if (isExistEmail)
            {
                throw new BaseException(ErrorsMessage.MSG_EXIST, "Email");
            }
            var isExistPhoneNumber = await _unitRep.GetAny(e => e.PhoneNumber == request.PhoneNumber && e.Id != request.Id);

            if (isExistPhoneNumber)
            {
                throw new BaseException(ErrorsMessage.MSG_EXIST, "Phone number");
            }

            Unit.Update(ref unit, request.Code, request.Name, request.Address, request.Email, request.PhoneNumber, request.Fax);

            await _unitOfWork.SaveChangesAsync();

            return unit.Id;
        }
    }
}
