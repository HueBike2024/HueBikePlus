using Core.Models.Base;
using Infrastructure.AggregatesModel.Authen.AccountAggregate;
using Infrastructure.AggregatesModel.MasterData.TripManagementAggregate.TripAggregate;
using Infrastructure.AggregatesModel.MasterData.UserAggregate.ComplainAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.AggregatesModel.MasterData.TripManagementAggregate.RateAggregate
{
    public class Rate : BaseEntity
    {
        public long RateStar { get; set; }
        public string Content { get; set; } = string.Empty;
        public long TripId { get; set; }
        public long SenderId { get; set; }

        public virtual Trip Trip { get; set; }
        public virtual User Sender { get; set; }

        public virtual ICollection<RateReply> Replys { get; set; }

        public Rate()
        {
            
        }

        public Rate(long rateStar, string content, long tripId, long senderId)
        {
            RateStar = rateStar;
            Content = content;
            TripId = tripId;
            SenderId = senderId;
        }

        //create rate
        public static Rate CreateRate(long rateStar, string content, long tripId, long senderId)
        {
            return new Rate
            {
                RateStar = rateStar,
                Content = content,
                TripId = tripId,
                SenderId = senderId
            };
        }

        //public method
        //Thêm mới reply
        public void AddeReply(RateReply newReply)
        {
            Replys.Add(newReply);
        }
        //Xóa bỏ reply
        public void RemoveReply(RateReply removeReply)
        {
            Replys.Remove(removeReply);
        }
    }
}
