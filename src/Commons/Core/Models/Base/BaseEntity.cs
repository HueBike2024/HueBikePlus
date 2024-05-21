using Core.Models.Interface;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Models.Base
{
    public class BaseEntity : IAuditEntity, IDeletable
    {
        //[Comment("Id bảng, khóa chính")]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public bool IsDeleted { get; set; } = false;
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public long? CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; } = DateTime.Now;
        public long? UpdatedBy { get; set; }
    }
}
