using Core.Extensions;
using Core.Models.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.EntityConfigurations
{
    public class BaseConfiguration
    {
        protected static void ConfigureBase<TEntity>(EntityTypeBuilder<TEntity> builder) where TEntity : class, IAuditEntity, IDeletable
        {
            builder.Property(e => e.IsDeleted).IsRequired().HasDefaultValue(false);
            builder.Property(e => e.CreatedDate).IsRequired();
            builder.Property(e => e.CreatedBy).HasMaxLength(16);
            builder.Property(e => e.UpdatedBy).HasMaxLength(16);

            builder.HasQueryFilter(e => !e.IsDeleted);

            builder.Property(e => e.CreatedDate).HasConversion(e => e, e => TokenExtensions.TimeZoneConversion(e));
            builder.Property(e => e.UpdatedDate).HasConversion(e => e, e => e.HasValue ? TokenExtensions.TimeZoneConversion(e.Value) : e);
        }
    }
}
