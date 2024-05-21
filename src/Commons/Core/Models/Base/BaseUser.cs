namespace Core.Models.Base
{
    public class BaseUser : BaseEntity
    {
        public string UserName { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
        public bool IsSuperAdmin { get; set; }
        public string TimeZone { get; set; }
    }
}