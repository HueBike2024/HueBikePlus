using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterData.Application.DTOs.Trip
{
    public class GetBikeInfoForTripResponse
    {
        public long Id { get; set; }
        public string StationName { get; set; }
        public string Location { get; set;}
    }
}
