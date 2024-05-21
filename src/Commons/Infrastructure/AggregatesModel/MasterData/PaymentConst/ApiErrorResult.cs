using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.AggregatesModel.MasterData.PaymentConst
{
    public class ApiErrorResult<T> : ApiResult<T>
    {
        public ApiErrorResult()
        {
            IsSuccessed = false;
        }

        public ApiErrorResult(string message)
        {
            IsSuccessed = false;
            Message = message;
        }
    }
}
