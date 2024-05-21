using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterData.Application.Sortings.Tickets
{
    public class TicketSorting
    {
        public static Dictionary<string, string> Mapping = new Dictionary<string, string>
        {
            { "ticketNo", "TicketNo" },
            { "userFullName", "userFullName" },
            { "userPhone", "UserPhone" },
            { "bikeName", "bikeName" },
            { "categoryTicketName", "CategoryTicketName" },
            { "ticketId", "TicketId" }
        };
    }
}
