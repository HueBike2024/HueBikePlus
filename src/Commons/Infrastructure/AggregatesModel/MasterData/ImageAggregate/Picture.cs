using Core.Models.Base;
using Infrastructure.AggregatesModel.MasterData.NotificationAggregate;
using Infrastructure.AggregatesModel.MasterData.UserAggregate.ComplainAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.AggregatesModel.MasterData.ImageAggregate
{
    public class Picture : BaseEntity
    {
        public string PublicId { get; set; }
        public long? NotificationId { get; set; }
        public long? ComplainId { get; set; }
        public string ImageUrl { get; set; }

        public virtual Notification Notification { get; set; }
        public virtual Complain Complain { get; set; }

        public Picture()
        {
            
        }

        public Picture(string publicId, long? notificationId, long? complainId, string imageUrl)
        {
            PublicId = publicId;
            NotificationId = notificationId;
            ComplainId = complainId;
            ImageUrl = imageUrl;
        }
    }
}
