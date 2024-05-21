using Core.Models.Base;
using Infrastructure.AggregatesModel.Authen.AccountAggregate;
using Infrastructure.AggregatesModel.MasterData.ImageAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.AggregatesModel.MasterData.UserAggregate.ComplainAggregate
{
    public class Complain : BaseEntity
    {
        private readonly List<ComplainReply> _replys = new List<ComplainReply>();
        public string Content { get; set; } = string.Empty;
        public string Image { get; set; } = string.Empty;
        public long? PictureId { get; set; }
        public long SenderId { get; set; }

        public virtual Picture Picture { get; set; }
        public virtual User ComplainSender { get; set; }

        public virtual ICollection<ComplainReply> ComplainReplys { get { return _replys; } }


        public Complain()
        {
            
        }

        public Complain(string content, string image, long senderId)
        {
            Content = content;
            Image = image;
            SenderId = senderId;
        }



        //Factory method:
        //Tạo mới đơn khiếu nại
        public static Complain CreateComplain(string content, string image, long senderId)
        {
            return new Complain
            {
                Content = content,
                Image = image,
                SenderId = senderId
            };
        }

        //public method
        //Thêm mới reply
        public void AddeReply(ComplainReply newReply)
        {
            _replys.Add(newReply);
        }
        //Xóa bỏ reply
        public void RemoveReply(ComplainReply removeReply)
        {
            _replys.Remove(removeReply);
        }
    }
}
