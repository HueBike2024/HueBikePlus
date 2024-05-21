using Core.Models.Base;
using Infrastructure.AggregatesModel.Authen.AccountAggregate;

namespace Infrastructure.AggregatesModel.MasterData.UserAggregate
{
    public class UserPayment : BaseEntity
    {
        public string VpcAccessCode { get; set; }
        public string VpcMerchant { get; set; }
        public string VpcMerchTxnRef { get; set; }
        public decimal VpcAmount { get; set; }
        public string VpcTicketNo { get; set; }
        public string VpcSecureHash { get; set; }
        public decimal Price { get; set; }
        public int Amount { get; set; }
        public decimal Discount { get; set; }
        public decimal TotalPrice { get; set; }
        public long UserId { get; set; }
        public string? Redirect { get; set; }
        public string? VpcResponseCode { get; set; }
        public long? MessageId { get; set; }
        public string? VpcCard { get; set; }
        public string? VpcCardNum { get; set; }
        public string? VpcOrderInfo { get; set; }
        public string? VpcTransactionNo { get; set; }

        public virtual User User { get; set; }

        public UserPayment()
        {
        }

        public UserPayment(string vpcAccessCode, string vpcMerchant, string vpcMerchTxnRef, decimal vpcAmount, string vpcTicketNo, string vpcSecureHash, decimal price, int amount, decimal discount, decimal totalPrice, long userId, string redirect)
        {
            VpcAccessCode = vpcAccessCode;
            VpcMerchant = vpcMerchant;
            VpcMerchTxnRef = vpcMerchTxnRef;
            VpcAmount = vpcAmount;
            VpcTicketNo = vpcTicketNo;
            VpcSecureHash = vpcSecureHash;
            Price = price;
            Amount = amount;
            Discount = discount;
            TotalPrice = totalPrice;
            UserId = userId;
            Redirect = redirect;
            VpcResponseCode = null;
            MessageId = null;
            VpcCard = null;
            VpcCardNum = null;
            VpcOrderInfo = null;
            VpcTransactionNo = null;
        }
    }
}
