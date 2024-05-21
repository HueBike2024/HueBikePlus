using Core.SeedWork;
using MasterData.Application.Sortings;

namespace MasterData.Application.Commands.UnitCommand
{
    public class ListUnitCommand: PagingQuery
    {
        public override Dictionary<string, string> GetFieldMapping()
        {
            return UnitSorting.Mapping;
        }
    }
}
