using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterData.Application.Sortings
{
    public class UserSorting
    {
        public static Dictionary<string, string> Mapping = new Dictionary<string, string>
        {
            { "fullName", "FullName" },
            { "username", "Username" },
            { "email", "Email" },
            { "phoneNumber", "PhoneNumber" },
            { "id", "Id" }
        };
    }
}
