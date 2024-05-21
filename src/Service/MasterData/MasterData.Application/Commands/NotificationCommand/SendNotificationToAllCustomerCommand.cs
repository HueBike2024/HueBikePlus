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
using Microsoft.EntityFrameworkCore;
using Infrastructure.AggregatesModel.MasterData.NotificationAggregate;

namespace MasterData.Application.Commands.NotificationCommand
{
    public class SendNotificationToAllCustomerCommand : IRequest<bool>
    {
        public long NotificationId { get; set; }
    }

    public class SendNotificationToAllCustomerCommandHandler : IRequestHandler<SendNotificationToAllCustomerCommand, bool>
    {
        public readonly IRepository<Notification> _notiRep;
        public readonly IRepository<User> _userRep;
        public readonly IRepository<UserNotification> _uNotiRep;
        public readonly IUnitOfWork _unitOfWork;

        public SendNotificationToAllCustomerCommandHandler(IRepository<Notification> notiRep, IUnitOfWork unitOfWork, IRepository<AuthenMedia> media, IRepository<User> userRep, IRepository<UserNotification> uNotiRep)
        {
            _notiRep = notiRep;
            _userRep = userRep;
            _unitOfWork = unitOfWork;
            _uNotiRep = uNotiRep;
        }
        public async Task<bool> Handle(SendNotificationToAllCustomerCommand request, CancellationToken cancellationToken)
        {
            var users = await _userRep.GetQuery().Where(e => e.IsSuperAdmin == false).ToListAsync();
            if (users == null)
            {
                throw new BaseException(ErrorsMessage.MSG_FAILED, "Không thể thực hiện");
            }

            var notification = await _notiRep.FindOneAsync(e => e.Id == request.NotificationId);
            if (notification == null)
            {
                throw new BaseException(ErrorsMessage.MSG_NOT_EXIST, "Thông báo");
            }

            foreach (var item in users)
            {
                var userNotification = new UserNotification(item.Id, request.NotificationId, false);
                _uNotiRep.Add(userNotification);
            }

            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}
