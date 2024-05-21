using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterData.Application.Commands.ComplainCommand
{
    public class DetailComplainCommand
    {
        public long ComplainId { get; set; }
        public long SenderId { get; set; }
    }
}
