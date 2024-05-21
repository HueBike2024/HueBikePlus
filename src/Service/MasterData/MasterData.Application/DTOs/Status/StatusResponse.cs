using Core.SeedWork.ExtendEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterData.Application.DTOs.Status
{
    public class StatusResponse : BaseExtendEntities
    {
        public int Index { get; set; }
        public long Id { get; set; }
        public string StatusName { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
}
