using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterData.Application.DTOs.Transaction
{
    public class PaymentResponse
    {
        public string TradingName { get; set; }
        public string VpcMerchTxnRef { get; set; }
        public string? CardNum { get; set; }
        public decimal TotalPrice { get; set; }
        public string UserFullName { get; set; }
        public string TradingStatus { get; set; }
        public DateTime TradingDate { get; set; }

    }
}
