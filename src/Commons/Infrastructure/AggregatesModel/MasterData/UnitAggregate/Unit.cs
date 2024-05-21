using Core.Models.Base;

namespace Infrastructure.AggregatesModel.MasterData.UnitAggregate
{
    public class Unit : BaseEntity
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Fax { get; set; }

        public Unit() { }
        public Unit(string code, string name, string address, string email, string phoneNumber, string fax)
        {
            Code = code;
            Name = name;
            Address = address;
            Email = email;
            PhoneNumber = phoneNumber;
            Fax = fax;
        }

        public static void Update(ref Unit unit, string code, string name, string address, string email, string phoneNumber, string fax)
        {
            unit.Code = code;
            unit.Name = name;
            unit.Address = address;
            unit.Email = email;
            unit.PhoneNumber = phoneNumber;
            unit.Fax = fax;
        }
    }
}
