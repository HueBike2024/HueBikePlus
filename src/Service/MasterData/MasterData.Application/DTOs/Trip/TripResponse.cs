﻿using Core.SeedWork.ExtendEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterData.Application.DTOs.Trip
{
    public class TripResponse : BaseExtendEntities
    {
        public int Index { get; set; }
        public long? TripId { get; set; }
        public long BikeId { get; set; }
        public string StartStation { get; set; }
        public string EndStation { get; set; }
        public string TripStatus { get; set; }
        public int MinutesTraveled { get; set; } = 0;
        public int ExcessMinutes { get; set; } = 0;
        public decimal TripPrice { get; set; }
        public string CategoryTicketName { get; set; }
    }
}
