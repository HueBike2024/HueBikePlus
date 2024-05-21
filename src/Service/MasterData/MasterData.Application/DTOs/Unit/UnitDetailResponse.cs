using Core.SeedWork.ExtendEntities;

namespace MasterData.Application.DTOs.Unit
{
    public class UnitDetailResponse: BaseExtendEntities
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Fax { get; set; }
    }
}
