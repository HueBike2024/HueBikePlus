using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Infrastructure.AggregatesModel.System;

namespace Infrastructure.EntityConfigurations.System
{
    public class ModuleConfiguration : BaseConfiguration, IEntityTypeConfiguration<Module>
    {
        public void Configure(EntityTypeBuilder<Module> builder)
        {
            builder.ToTable("SYS_Module");

            builder.HasKey(x => new { x.Id });

            builder.Property(x => x.Name).IsRequired().HasMaxLength(50);
            builder.Property(x => x.Segment).IsRequired().HasMaxLength(50);
            builder.Property(x => x.Message).HasMaxLength(2000);
            builder.Property(x => x.IsActive).IsRequired().HasDefaultValue(true);

            builder.HasIndex(x => x.Name).IsUnique();
            builder.HasIndex(x => x.Segment).IsUnique();

            ConfigureBase(builder);
        }
    }
}
