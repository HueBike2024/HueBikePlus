using Infrastructure.AggregatesModel.Authen.PermissionAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.EntityConfigurations.Authen.PermissionConfig
{
    public class PermissionConfiguration : BaseConfiguration, IEntityTypeConfiguration<Permission>
    {
        public void Configure(EntityTypeBuilder<Permission> builder)
        {
            builder.ToTable("AUTH_Permission");
            builder.HasKey(x => new { x.Id });

            builder.Property(x => x.Name).HasMaxLength(250).IsRequired();
            builder.Property(x => x.Description).HasMaxLength(500);

            ConfigureBase(builder);
        }
    }
}