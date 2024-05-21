using Core.Exceptions;
using Core.Interfaces.Database;
using Core.Properties;
using Core.SeedWork.Repository;
using Infrastructure.AggregatesModel.Authen.AccountAggregate;
using MasterData.Application.Commands.AccountCommand;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User = Infrastructure.AggregatesModel.Authen.AccountAggregate.User;

namespace MasterData.Application.Commands.UserCommand
{
    public class UnLockUserCommand : IRequest<bool>
    {
        public long Id { get; set; }
    }

    public class UnLockUserCommandHandler : IRequestHandler<UnLockUserCommand, bool>
    {

        private readonly IRepository<User> _userRep;
        private readonly IUnitOfWork _unitOfWork;

        public UnLockUserCommandHandler(IRepository<User> userRep, IUnitOfWork unitOfWork)
        {
            _userRep = userRep;
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(UnLockUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRep.FindOneAsync(e => e.Id == request.Id);

            if (user == null)
            {
                throw new BaseException(ErrorsMessage.MSG_NOT_EXIST, "Người dùng");
            }

            User.UnLockUser(ref user);


            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}
