using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterData.Application.DTOs.User
{
    public class LockUserResponse
    {
        public long Id { get; set; }
        public bool IsLocked { get; set; }
        public bool IsSuperAdmin { get; set; }

    }
}
