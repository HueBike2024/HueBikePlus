using Core.Models.Base;
using Infrastructure.AggregatesModel.MasterData.BikeManagementAggregate.BikeAggregate;
using Infrastructure.AggregatesModel.MasterData.BikeManagementAggregate.StationAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.AggregatesModel.MasterData.BikeManagementAggregate.BikeStationAggregate
{
    public class BikeStation : BaseEntity
    {
        public long BikeId { get; set; }
        public long StationId { get; set; }

        public virtual Bike Bike { get; set; }
        public virtual Station Station { get; set; }

        public BikeStation()
        {
            
        }

        public BikeStation(long bikeId, long stationId, DateTime changeTime)
        {
            BikeId = bikeId;
            StationId = stationId;
        }

        //add new change
        public static BikeStation AddBikeStation(long bikeId, long stationId)
        {
            return new BikeStation
            {
                BikeId = bikeId,
                StationId = stationId,
            };
        }
    }
}
