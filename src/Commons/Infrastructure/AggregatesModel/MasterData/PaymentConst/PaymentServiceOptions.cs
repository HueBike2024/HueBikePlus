using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.AggregatesModel.MasterData.PaymentConst
{
    public class PaymentServiceOptions
    {
        public string HashKey { get; set; }
        public string AccessCode { get; set; }
        public string MerchantId { get; set; }
        public string Currency { get; set; }
        public string PaymentUrl { get; set; }
    }
}
