using Core.SeedWork.ExtendEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterData.Application.DTOs.CategoryTicket
{
    public class CategoryticketResponse : BaseExtendEntities
    {
        public int Index { get; set; }
        public long Id { get; set; }
        public string CategoryTicketName { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public double UserTime { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
