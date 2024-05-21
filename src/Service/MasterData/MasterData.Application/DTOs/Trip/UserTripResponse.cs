using Core.SeedWork.ExtendEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterData.Application.DTOs.Trip
{
    public class UserTripResponse :BaseExtendEntities
    {
        public int Index { get; set; }
        public long BikeId { get; set; }
        public string TripStatus { get; set; }
        public int BikePower { get; set; }
        public long TripPrice { get; set; }
        public string CategoryTicketName { get; set; }
    }
}
