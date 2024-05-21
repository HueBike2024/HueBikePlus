using Infrastructure.AggregatesModel.Authen.PermissionAggregate;

namespace Infrastructure.AggregatesModel.Authen.FunctionAggregate
{
    public class Function
    {
        public long Id { get; set; }
        public long ModuleId { get; set; }
        public string Name { get; set; }
        public string ControllerName { get; set; }
        public string Description { get; set; }
        public string Permissions { get; set; } // Ví dụ: C,R,U,D
        public string Url { get; set; }
        public int? DisplayOrder { get; set; }
        public bool IsDisplay { get; set; }

        public virtual Module Module { get; set; }
        public virtual ICollection<PermissionFunction> PermissionFunctions { get; set; }
        public virtual ICollection<UserFunction> UserFunctions { get; set; }

        public Function()
        {

        }
    }
}