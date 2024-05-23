using Core.SeedWork.ExtendEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterData.Application.DTOs.Trip
{
    public class ListTripResponse : BaseExtendEntities
    {
        public int Index { get; set; }
        public string BikeCode { get; set; }
        public DateTime StatTime { get; set; }
        public bool isEnd { get; set; }
        public bool isDebt { get; set; }
        public string StartStation { get; set; }
        public int MinutesTraveled { get; set; } = 0;
        public decimal TripPrice { get; set; }
    }
}
