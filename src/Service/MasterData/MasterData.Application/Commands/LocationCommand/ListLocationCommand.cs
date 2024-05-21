using Core.SeedWork;
using MasterData.Application.Sortings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterData.Application.Commands.LocationCommand
{
    public class ListLocationCommand : PagingQuery
    {
        public override Dictionary<string, string> GetFieldMapping()
        {
            return LocationSorting.Mapping;
        }
    }
}
