using Infrastructure.AggregatesModel.Authen.AccountAggregate;

namespace Infrastructure.AggregatesModel.Authen.FunctionAggregate
{
    public class UserFunction
    {
        public long UserId { get; set; }
        public long FunctionId { get; set; }
        public string Permissions { get; set; } // Ví dụ: C,R,U,D
        public DateTime UpdatedDate { get; set; } = DateTime.UtcNow;

        public virtual User User { get; set; }
        public virtual Function Function { get; set; }

        public UserFunction()
        {

        }

        public UserFunction(long functionId, string permissions)
        {
            FunctionId = functionId;
            Permissions = permissions;
        }
    }
}