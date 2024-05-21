using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterData.Application.DTOs.Transaction
{
    public class WalletInfoResponse
    {
        public decimal? CurrentPoint {  get; set; }
        public decimal? DebtCharge { get; set; }
    }
}
