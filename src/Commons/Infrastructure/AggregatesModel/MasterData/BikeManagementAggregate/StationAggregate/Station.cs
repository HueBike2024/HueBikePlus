using Core.Models.Base;
using Infrastructure.AggregatesModel.MasterData.BikeManagementAggregate.BikeAggregate;
using Infrastructure.AggregatesModel.MasterData.BikeManagementAggregate.BikeStationAggregate;
using Infrastructure.AggregatesModel.MasterData.BikeManagementAggregate.MapLocationAggregate;
using Infrastructure.AggregatesModel.MasterData.StatusAggregate;
using Infrastructure.AggregatesModel.MasterData.TripManagementAggregate.TripAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.AggregatesModel.MasterData.BikeManagementAggregate.StationAggregate
{
    public class Station : BaseEntity
    {
        public string StationName { get; set; }
        public int QuantityAvaiable { get; set; }
        public int NumOfSeats { get; set; }
        public string LocationName { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public long StatusId { get; set; }

        
        public virtual Status Status { get; set; }
        public virtual ICollection<Bike> Bikes { get; set; }
        public virtual ICollection<Trip> EndTrips { get; set; }
        public virtual ICollection<Trip> StartTrips { get; set; }


        public Station()
        {

        }

        public Station(string stationName, string locationName, double longitude, double latitude, int numOfSeats, long statusId)
        {
            StationName = stationName;
            LocationName = locationName;
            Longitude = longitude;
            Latitude = latitude;
            NumOfSeats = numOfSeats;
           
            StatusId = statusId;

        }

        //Add Station
        public static Station CreateStation(string stationName, int quantityAvaiable, int numOfSeats, long statusId)
        {
            return new Station
            {
                StationName = stationName,
                QuantityAvaiable = quantityAvaiable,
                NumOfSeats = numOfSeats,               
                StatusId = statusId
            };
        }

        // xóa trạm
        public static void DeleteStation(ref Station station)
        {
            station.IsDeleted = true;
        }

    }
}
