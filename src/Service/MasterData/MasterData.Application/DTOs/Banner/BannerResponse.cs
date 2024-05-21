using Core.SeedWork.ExtendEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterData.Application.DTOs.Banner
{
    public class BannerResponse : BaseExtendEntities
    {
        public int Index { get; set; }
        public long Id { get; set; }
        public string Tilte { get; set; }
        public string Image { get; set; }
        public string Type { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
