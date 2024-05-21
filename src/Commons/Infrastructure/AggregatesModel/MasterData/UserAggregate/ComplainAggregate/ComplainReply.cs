using Core.Models.Base;
using Infrastructure.AggregatesModel.Authen.AccountAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.AggregatesModel.MasterData.UserAggregate.ComplainAggregate
{
    public class ComplainReply : BaseEntity
    {

        public string Content { get; set; } = string.Empty;
        public long ComplainId { get; set; }
        public long SenderId { get; set; }

        public Complain Complain { get; set; }
        public User Sender { get; set; }

        public ComplainReply()
        {
            
        }

        public ComplainReply(string content, long complainId, long senderId)
        {
            Content = content;
            ComplainId = complainId;
            SenderId = senderId;
        }



        //factory
        //Tạo mới câu trả lời
        public static ComplainReply CreateReply(string content, long complainId, long accountId)
        {
            return new ComplainReply
            {
                Content = content,
                ComplainId = complainId,
                SenderId = accountId
            };
        }

        //public method
        //Chỉnh sửa reply
        public void UpdateReplyContent(string updateContent)
        {
            Content = updateContent;
        }
    }
}
