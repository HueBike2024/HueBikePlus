using MasterData.Application.DTOs.Complain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterData.Application.DTOs.Station
{
    public class StationDetailResponse
    {
        public long Id { get; set; }
        public string StationName { get; set; }
        public int QuantityAvaiable { get; set; }
        public int NumOfSeats { get; set; }
        
        public string LocationName { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }

        public long StatusId { get; set; }

        public string StatusName { get; set; }
        public long NumOfActiveBikes { get; set; }
        public long NumOfOtherBikes { get; set; }
        public List<BikeList> Bikes { get; set; }



    }
    public class BikeList
    {
        public long Id { get; set; }
        public string BikeName { get; set; }
       
        public string StatusName { get; set; }
    }

}
