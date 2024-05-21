using Core.Models.Base;
using Core.Models.Interface;
using Infrastructure.AggregatesModel.Authen.AccountAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.AggregatesModel.MasterData.ImageAggregate
{
    public class Avatar : BaseEntity
    {
        public string PublicId { get; set; }
        public string ImageUrl { get; set; }

        public virtual User User { get; set; }

        public Avatar()
        {
            
        }

        public Avatar(string publicId, string imageUrl)
        {
            PublicId = publicId;
            ImageUrl = imageUrl;
        }
    }
}
