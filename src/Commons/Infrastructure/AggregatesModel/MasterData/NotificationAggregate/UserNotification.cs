using Core.Models.Base;
using Infrastructure.AggregatesModel.Authen.AccountAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.AggregatesModel.MasterData.NotificationAggregate
{
    public class UserNotification : BaseEntity
    {
        public long UserId { get; set; }
        public long NotificationId { get; set; }
        public bool IsRead { get; set; }

        public virtual User User { get; set; }
        public virtual Notification Notification { get; set; }

        public UserNotification()
        {
            
        }

        public UserNotification(long userId, long notificationId, bool isRead)
        {
            UserId = userId;
            NotificationId = notificationId;
            IsRead = isRead;
        }
    }
}
