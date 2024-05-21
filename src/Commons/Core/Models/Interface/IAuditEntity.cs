using System;

namespace Core.Models.Interface
{
    public interface IAuditEntity
    {
        DateTime CreatedDate { get; set; }
        long? CreatedBy { get; set; }
        DateTime? UpdatedDate { get; set; }
        long? UpdatedBy { get; set; }        
    }
}
