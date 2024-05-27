using Core.Models.Base;
using Infrastructure.AggregatesModel.MasterData.BikeManagementAggregate.StationAggregate;
using Infrastructure.AggregatesModel.MasterData.TripManagementAggregate.RateAggregate;
using Infrastructure.AggregatesModel.MasterData.TripManagementAggregate.TicketAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.AggregatesModel.MasterData.TripManagementAggregate.TripAggregate
{
    public class Trip : BaseEntity
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsEnd { get; set; }
        public bool IsDebt { get; set; }
        public long StartStationId { get; set;}
        public long? EndStationId { get; set; } = null;
        public long TicketId { get; set;}
        public int MinutesTraveled { get; set; } = 0;
        public int ExcessMinutes { get; set; } = 0; 
        public decimal TripPrice { get; set; } = 0;
        public bool IsSentNotificationDeft { get; set; } = false;

        public virtual Station StartStation { get; set; }
        public virtual Station EndStation { get; set; }

        public virtual Ticket Ticket { get; set; }
        public virtual Rate Rate { get; set; }

        public Trip()
        {
            
        }

        public Trip(bool isEnd, bool isDebt, long startStationId, long ticketId, decimal tripPrice)
        {
            StartDate = DateTime.Now;
            EndDate = DateTime.Now;
            IsEnd = isEnd;
            IsDebt = isDebt;
            StartStationId = startStationId;
            TicketId = ticketId;
            TripPrice = tripPrice;
        }
    }
}
