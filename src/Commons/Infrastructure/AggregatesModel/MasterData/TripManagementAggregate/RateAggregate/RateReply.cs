using Core.Models.Base;
using Infrastructure.AggregatesModel.Authen.AccountAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.AggregatesModel.MasterData.TripManagementAggregate.RateAggregate
{
    public class RateReply :BaseEntity
    {
        public string Content { get; set; }
        public long RateId { get; set; }
        public long SenderId { get; set; }

        public virtual Rate Rate { get; set; }
        public virtual User Sender { get; set; }

        public RateReply()
        {
            
        }

        public RateReply(string content, long rateId, long senderId)
        {
            Content = content;
            RateId = rateId;
            SenderId = senderId;
        }

        //create reply
        public static RateReply CreateReply(string content, long rateId, long senderId)
        {
            return new RateReply
            {
                Content = content,
                RateId = rateId,
                SenderId = senderId
            };
        }
    }
}
