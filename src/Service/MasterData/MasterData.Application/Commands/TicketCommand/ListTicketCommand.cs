using Core.SeedWork;
using MasterData.Application.Sortings.Tickets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterData.Application.Commands.TicketCommand
{
    public class ListTicketCommand : PagingQuery
    {
        public long? StatusId { get; set; }
        public DateTime? BookingDate { get; set; }
        public override Dictionary<string, string> GetFieldMapping()
        {
            return TicketSorting.Mapping;
        }
    }
}
