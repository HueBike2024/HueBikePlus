using Core.SeedWork.ExtendEntities;

namespace MasterData.Application.DTOs.Attribute
{
    public class AttributeResponse : BaseExtendEntities
    {
        public long Index { get; set; }
        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Level { get; set; }
        public int DisplayOrder { get; set; }
        public long? ParentAttributeId { get; set; }
        public string ParentCode { get; set; }
        public string ParentName { get; set; }
        public bool IsDisplay { get; set; }
        public bool IsRequired { get; set; }
        public bool IsCommon { get; set; }
    }
}
