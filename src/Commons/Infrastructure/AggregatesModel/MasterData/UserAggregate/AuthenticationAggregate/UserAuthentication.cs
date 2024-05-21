using Core.Models.Base;
using Infrastructure.AggregatesModel.Authen.AccountAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.AggregatesModel.MasterData.UserAggregate.AuthenticationAggregate
{
    public class UserAuthentication : BaseEntity
    {
        public string? CardId { get; set; }
        public DateTime? IssueDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        //public string FrontOfCard { get; set; }
        //public string BackOfCard { get; set; }
        public long UserId { get; set; }

        public virtual User User { get; set; }

        public UserAuthentication()
        {
            
        }

        public UserAuthentication(string cardId, DateTime? issueDate, DateTime expiryDate, long userId)
        {
            CardId = cardId;
            IssueDate = issueDate;
            ExpiryDate = expiryDate;
            UserId = userId;
        }
    }
}
