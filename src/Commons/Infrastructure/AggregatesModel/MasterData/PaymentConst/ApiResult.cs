using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.AggregatesModel.MasterData.PaymentConst
{
    public class ApiResult<T>
    {
        public bool IsSuccessed { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }

        public ApiResult() { }

        public ApiResult(T data, string message = null)
        {
            IsSuccessed = true;
            Data = data;
            Message = message;
        }
    }
}
