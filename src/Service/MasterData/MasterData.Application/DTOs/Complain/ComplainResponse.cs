using Core.SeedWork.ExtendEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterData.Application.DTOs.Complain
{
    public class ComplainResponse : BaseExtendEntities
    {
        public int Index { get; set; }
        public long Id { get; set; }
        public long SenderId { get; set; }
        public string SenderUsername { get; set; }
        public string Image {  get; set; }
        public string Content { get; set;}
        public DateTime CreatedDate { get; set; } 

    }
}
