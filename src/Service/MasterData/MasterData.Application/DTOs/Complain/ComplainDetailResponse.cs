using Core.SeedWork.ExtendEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterData.Application.DTOs.Complain
{
    public class ComplainDetailResponse : BaseExtendEntities
    {
        public long Id { get; set; }
        public long SenderId { get; set; }
        public string SenderUsername { get; set; }
        public string Image { get; set; }
        public string Content { get; set; }
        public DateTime CreatedDate { get; set; }
        public ICollection<Reply>? Replys { get; set; }
    }
    public class Reply
    {
        public int Index { get; set; }
        public long Id { get; set; }
        public long ComplainId { get; set; }
        public long SenderId { get; set; }
        public string Content { get; set; }
        public DateTime CreatedDate { get; set; } 
    }
}
