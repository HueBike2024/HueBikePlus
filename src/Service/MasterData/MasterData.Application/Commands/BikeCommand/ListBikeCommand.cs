using Core.SeedWork;
using MasterData.Application.Sortings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterData.Application.Commands.BikeCommand
{
    public class ListBikeCommand : PagingQuery
    {
        public long? StatusId { get; set; }
        public long? StationId { get; set; }
        public override Dictionary<string, string> GetFieldMapping()
        {
            return BikeSorting.Mapping;
        }
    }
}
