using Infrastructure.AggregatesModel.Authen.AccountAggregate;

namespace Infrastructure.AggregatesModel.Authen.PermissionAggregate
{
    public class UserPermission
    {
        public long UserId { get; set; }
        public long PermissionId { get; set; }

        public virtual User User { get; set; }
        public virtual Permission Permission { get; set; }

        public UserPermission()
        {

        }
        public UserPermission(long permissionId)
        {
            PermissionId = permissionId;
        }
    }
}