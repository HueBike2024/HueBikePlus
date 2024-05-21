using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterData.Application.Sortings
{
    public class ComplainSorting
    {
        public static Dictionary<string, string> Mapping = new Dictionary<string, string>
        {
            { "complainId", "ComplainId" },
            { "senderId", "SenderId" },
            { "username", "Username" },
        };
    }
}
