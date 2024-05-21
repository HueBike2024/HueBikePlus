using Core.SeedWork.ExtendEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterData.Application.DTOs.Location
{
    public class LocationResponse : BaseExtendEntities
    {
        public int Index { get; set; }
        public long Id { get; set; }
        public string LocationName { get; set; }
        public double Logitude { get; set; }
        public double Latitude { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
