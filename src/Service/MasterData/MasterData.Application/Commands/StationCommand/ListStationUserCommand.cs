using Core.SeedWork;
using MasterData.Application.Sortings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterData.Application.Commands.StationCommand
{
    public class ListStationUserCommand : PagingQuery
    {
        public double UserLatitude { get; set; }
        public double UserLongitude { get; set; }



        public override Dictionary<string, string> GetFieldMapping()
        {
            return StationSorting.Mapping;
        }
    }
}

