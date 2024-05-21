using Core.Models.Base;
using Infrastructure.AggregatesModel.Authen.AccountAggregate;

namespace Infrastructure.AggregatesModel.Authen
{
    public class AuthenMedia : BaseMedia
    {
        public virtual User User { get; set; }
    }
}
