using Infrastructure.AggregatesModel.Authen.FunctionAggregate;

namespace Infrastructure.AggregatesModel.Authen.PermissionAggregate
{
    public class PermissionFunction
    {
        public long PermissionId { get; set; }
        public long FunctionId { get; set; }
        public string Permissions { get; set; } // Ví dụ: C,R,U,D

        public virtual Permission Permission { get; set; }
        public virtual Function Function { get; set; }

        public PermissionFunction()
        {

        }

        public PermissionFunction(long functionId, string permissions)
        {
            FunctionId = functionId;
            Permissions = permissions;
        }
    }
}