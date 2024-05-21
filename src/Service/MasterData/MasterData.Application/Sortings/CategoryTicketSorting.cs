using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterData.Application.Sortings
{
    public class CategoryTicketSorting
    {
        public static Dictionary<string, string> Mapping = new Dictionary<string, string>
        {
            { "categoryTicketId", "CategoryTicketId" },
            { "categoryTicketName", "CategoryTicketName" },
        };
    }
}
