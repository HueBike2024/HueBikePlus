using Core.SeedWork.ExtendEntities;

namespace MasterData.Application.DTOs.Station
{
    public class StationResponse : BaseExtendEntities
    {
        public int Index { get; set; }
        public long Id { get; set; }

        public string StationName { get; set; }
        public int QuantityAvaiable { get; set; }
        public int NumOfSeats { get; set; }

        public string LocationName { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public double? Distance { get; set; }
       
        
        public string StatusName { get; set; }
        public long StatusId { get; set; }
        
    }
}
