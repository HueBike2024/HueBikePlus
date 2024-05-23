using Core.SeedWork;
using MasterData.Application.Sortings;

namespace MasterData.Application.Commands.EventCommand
{
    public class ListEventCommand : PagingQuery
    {
        public override Dictionary<string, string> GetFieldMapping()
        {
            return EventSorting.Mapping;
        }
    }
}
