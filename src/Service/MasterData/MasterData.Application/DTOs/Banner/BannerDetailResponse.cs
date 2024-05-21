using Core.SeedWork.ExtendEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterData.Application.DTOs.Banner
{
    public class BannerDetailResponse : BaseExtendEntities
    {
        public long Id { get; set; }
        public string Tilte { get; set; }
        public string Image { get; set; }
        public string Type { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
