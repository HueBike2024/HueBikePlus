using Core.SeedWork;
using MasterData.Application.Sortings.Tickets;
using MasterData.Application.Sortings.Trips;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterData.Application.Commands.TripCommand
{
    public class ListTripCommand : PagingQuery
    {
       public bool IsEnd { get; set; }
       public bool IsDebt { get; set; }

        public override Dictionary<string, string> GetFieldMapping()
        {
            return TripSorting.Mapping;
        }
    }
}
