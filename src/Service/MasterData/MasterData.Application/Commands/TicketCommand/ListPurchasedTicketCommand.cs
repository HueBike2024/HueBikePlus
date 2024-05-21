using Core.SeedWork;
using MasterData.Application.Sortings;
using MasterData.Application.Sortings.Tickets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterData.Application.Commands.TicketCommand
{
    public class ListPurchasedTicketCommand : PagingQuery
    {
        public long UserId { get; set; }
        public long? StatusId { get; set; }
        public override Dictionary<string, string> GetFieldMapping()
        {
            return PurchasedTicketSorting.Mapping;
        }
    }
}
