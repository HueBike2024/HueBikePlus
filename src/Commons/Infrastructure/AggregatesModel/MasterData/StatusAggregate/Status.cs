using Core.Models.Base;
using Infrastructure.AggregatesModel.Authen.AccountAggregate;
using Infrastructure.AggregatesModel.MasterData.BikeManagementAggregate.BikeAggregate;
using Infrastructure.AggregatesModel.MasterData.BikeManagementAggregate.StationAggregate;
using Infrastructure.AggregatesModel.MasterData.TripManagementAggregate.TicketAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.AggregatesModel.MasterData.StatusAggregate
{
    public class Status : BaseEntity
    {
        public string StatusName { get; set; } = string.Empty;

        public virtual ICollection<User> Users { get;  set; }
        public virtual ICollection<Bike> Bikes { get; set; }
        public virtual ICollection<Station> Stations { get; set; }
        public virtual ICollection<Ticket> Tickets { get; set; }

        public Status()
        {
            
        }

        public Status( string statusName)
        {
            StatusName = statusName;
        }

        //Add new status
        public static Status AddStatus(string statusName)
        {
            return new Status
            {
                StatusName = statusName,
            };
        }
    }
}
