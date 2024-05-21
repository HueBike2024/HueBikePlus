using Core.SeedWork.ExtendEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterData.Application.DTOs.Transaction
{
    public class ListTransactionResponse : BaseExtendEntities
    {
        public int Index { get; set; }
        public long Id { get; set; }
        public string? TransactionType { get; set; }
        public decimal Point { get; set; }
        public string TraderName { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
