using Core.Exceptions;
using Core.Interfaces.Database;
using Core.Properties;
using Core.SeedWork.Repository;
using Infrastructure.AggregatesModel.Authen.AccountAggregate;
using Infrastructure.AggregatesModel.Authen;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.AggregatesModel.MasterData.NotificationAggregate;

namespace MasterData.Application.Commands.NotificationCommand
{
    public class SendNotificationByCustomerIdCommand : IRequest<bool>
    {
        public long UserId { get; set; }
        public long NotificationId { get; set; }
    }

    public class SendNotificationByCustomerIdCommandHandler : IRequestHandler<SendNotificationByCustomerIdCommand, bool>
    {
        public readonly IRepository<Notification> _notiRep;
        public readonly IRepository<User> _userRep;
        public readonly IRepository<UserNotification> _uNotiRep;
        public readonly IUnitOfWork _unitOfWork;

        public SendNotificationByCustomerIdCommandHandler(IRepository<Notification> notiRep, IUnitOfWork unitOfWork, IRepository<AuthenMedia> media, IRepository<User> userRep, IRepository<UserNotification> uNotiRep)
        {
            _notiRep = notiRep;
            _userRep = userRep;
            _unitOfWork = unitOfWork;
            _uNotiRep = uNotiRep;
        }
        public async Task<bool> Handle(SendNotificationByCustomerIdCommand request, CancellationToken cancellationToken)
        {
            var users = await _userRep.FindOneAsync(e => e.Id == request.UserId);
            if (users == null)
            {
                throw new BaseException(ErrorsMessage.MSG_NOT_EXIST, "Người dùng");
            }

            var notification = await _notiRep.FindOneAsync(e => e.Id == request.NotificationId);
            if (notification == null)
            {
                throw new BaseException(ErrorsMessage.MSG_NOT_EXIST, "Thông báo");
            }

            
            var userNotification = new UserNotification(request.UserId, request.NotificationId, false);
            _uNotiRep.Add(userNotification);

            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}
