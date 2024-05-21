using Core.SeedWork.ExtendEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterData.Application.DTOs.Bike
{
    public class BikeResponse : BaseExtendEntities
    {
        public int Index { get; set; }
        public long Id { get; set; }
        public string BikeName { get; set; }
        public long StationId { get; set; }
        public string StationName { get; set; }
        public string? Location { get; set; }
        public string? PathQr { get; set; }
        public string? QrCodeImage { get; set; }
        public int? Power { get; set; }
        public int RentalQuantity { get; set; }
        public string StatusName { get; set; }
        public long StatusId { get; set; }
        public string ActiveStatus { get; set; }

    }
}
