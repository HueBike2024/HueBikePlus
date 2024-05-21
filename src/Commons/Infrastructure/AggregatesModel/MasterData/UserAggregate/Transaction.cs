using Core.Models.Base;
using Infrastructure.AggregatesModel.Authen.AccountAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.AggregatesModel.MasterData.UserAggregate
{
    public class Transaction : BaseEntity
    {
        public string? TransactionType { get; set; }
        public string TransactionCode { get; set; }
        public decimal Point {  get; set; }
        public long UserId { get; set; }
        public bool IsSuccess { get; set; }

        public virtual User User { get; set; }

        public Transaction()
        {
            
        }

        public Transaction(string transactionType, string transactionCode, decimal point, long userId, bool isSuccess)
        {
            TransactionType = transactionType;
            TransactionCode = transactionCode;
            Point = point;
            UserId = userId;
            IsSuccess = isSuccess;
            CreatedDate = DateTime.Now;
        }
    }
}
