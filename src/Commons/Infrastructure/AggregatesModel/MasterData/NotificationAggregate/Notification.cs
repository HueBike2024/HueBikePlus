using Core.Models.Base;
using Infrastructure.AggregatesModel.Authen.AccountAggregate;
using Infrastructure.AggregatesModel.MasterData.ImageAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.AggregatesModel.MasterData.NotificationAggregate
{
    public class Notification : BaseEntity
    {

        public string Title { get;  set; } = string.Empty;
        public string Image { get; set; } = string.Empty;
        public long? PictureId { get; set; }
        public string Content { get; set; } = string.Empty;
        public long? UserId { get; set; }

        public virtual Picture Picture { get; set; }
        public virtual ICollection<UserNotification> UserNotifications { get; set; }
         

        public Notification()
        {
            
        }

        public Notification(string title, string image, string content, long senderId)
        {
            Title = title;
            Image = image;
            Content = content;
            UserId = senderId;
        }



        //factory
        //Tạo thông báo mới
        public Notification(string title, string image, string content)
        {

            Title = title;
            Image = image;
            Content = content;
        }
    }
}
