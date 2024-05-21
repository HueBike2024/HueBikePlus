using Core.Properties;
using Core.Interfaces.Database;
using Core.Exceptions;
using Core.SeedWork.Repository;
using Unit = Infrastructure.AggregatesModel.MasterData.UnitAggregate.Unit;
using MediatR;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace MasterData.Application.Commands.UnitCommand
{
    public class CreateUnitCommand: IRequest<long>
    {
        public string Code { get; set; }
        
       
        public string Name { get; set; }
        public string Address { get; set; }
        [EmailAddress(ErrorMessage = "Email incorrect formatted")] //Todo: sau này sẽ update mã hóa thông tin 
        public string Email { get; set; }
        [Phone(ErrorMessage = "Phone number incorrect formatted")] //Todo: sau này sẽ update mã hóa thông tin 
        public string PhoneNumber { get; set; }
        public string Fax { get; set; }
    }
    public class CreateUnitCommandHandler: IRequestHandler<CreateUnitCommand, long>
    {
        public readonly IRepository<Unit> _unitRep;
        public readonly IUnitOfWork _unitOfWork;

        public CreateUnitCommandHandler(IRepository<Unit> unitRep, IUnitOfWork unitOfWork)
        {
            _unitRep = unitRep;
            _unitOfWork = unitOfWork;
        }
        public async Task<long> Handle(CreateUnitCommand request, CancellationToken cancellationToken)
        {
            var isCode = await _unitRep.GetAny(e => e.Code.Trim().ToLower() == request.Code.Trim().ToLower());

            if (isCode)
            {
                throw new BaseException(ErrorsMessage.MSG_EXIST, "Code");
            }

            var isExistName = await _unitRep.GetAny(e => e.Name.Trim().ToLower() == request.Name.Trim().ToLower());

            if (isExistName)
            {
                throw new BaseException(ErrorsMessage.MSG_EXIST, "Name");
            }

            var isExistEmail = await _unitRep.GetAny(e => e.Email == request.Email);

            if (isExistEmail)
            {
                throw new BaseException(ErrorsMessage.MSG_EXIST, "Email");
            }
            var isExistPhoneNumber = await _unitRep.GetAny(e => e.PhoneNumber == request.PhoneNumber);

            if (isExistPhoneNumber)
            {
                throw new BaseException(ErrorsMessage.MSG_EXIST, "Phone number");
            }
            var unit = new Unit(request.Code, request.Name, request.Address, request.Email, request.PhoneNumber, request.Fax);

            _unitRep.Add(unit);

            await _unitOfWork.SaveChangesAsync();
            
            return unit.Id;
        }
    }
}
