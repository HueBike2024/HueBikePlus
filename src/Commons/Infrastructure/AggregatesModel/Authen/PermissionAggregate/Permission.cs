using Core.Models.Base;

namespace Infrastructure.AggregatesModel.Authen.PermissionAggregate
{
    public class Permission : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public virtual ICollection<PermissionFunction> PermissionFunctions { get; set; }
        public virtual ICollection<UserPermission> UserPermissions { get; set; }

        public Permission()
        {

        }
        public Permission(string name, string description, ICollection<PermissionFunction> permissionFunctions)
        {
            Name = name;
            Description = description;
            PermissionFunctions = permissionFunctions;
        }

        public static void Update(ref Permission permission, string name, string description) {
            permission.Name = name;
            permission.Description = description;   
        }
    }
}