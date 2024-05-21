using Core.Exceptions;
using Core.Interfaces.Database;
using Core.Properties;
using Core.SeedWork.Repository;
using Infrastructure.AggregatesModel.Authen.AccountAggregate;
using MasterData.Application.Commands.UnitCommand;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User = Infrastructure.AggregatesModel.Authen.AccountAggregate.User;

namespace MasterData.Application.Commands.AccountCommand
{
    public class LockUserCommand : IRequest<bool>
    {
        public long Id { get; set; }
        public bool IsSuperAdmin { get; set; }
    }

    public class LockUserCommandHandler : IRequestHandler<LockUserCommand, bool>
    {

        private readonly IRepository<User> _userRep;
        private readonly IUnitOfWork _unitOfWork;

        public LockUserCommandHandler(IRepository<User> userRep, IUnitOfWork unitOfWork)
        {
            _userRep = userRep;
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(LockUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRep.FindOneAsync(e => e.Id == request.Id);

            if (user == null)
            {
                throw new BaseException(ErrorsMessage.MSG_NOT_EXIST, "Người dùng");
            }

            bool isSuperAdmin = await _userRep.GetAny(e => e.Id == request.Id && e.IsSuperAdmin == request.IsSuperAdmin);

            if (isSuperAdmin)
            {
                throw new BaseException(ErrorsMessage.MSG_FAILED, "Không thể xóa người quản lí khác!");
            }

            User.LockUser(ref user);


            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}
