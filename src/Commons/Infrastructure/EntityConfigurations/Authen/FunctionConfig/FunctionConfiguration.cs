using Infrastructure.AggregatesModel.Authen.FunctionAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.EntityConfigurations.Authen.FunctionConfig
{
    public class FunctionConfiguration : IEntityTypeConfiguration<Function>
    {
        public void Configure(EntityTypeBuilder<Function> builder)
        {
            builder.ToTable("AUTH_Function");
            builder.HasKey(x => new { x.Id });

            builder.Property(x => x.Name).HasMaxLength(250).IsRequired();
            builder.Property(x => x.ControllerName).HasMaxLength(128).IsRequired();
            builder.Property(x => x.Description).HasMaxLength(500);
            builder.Property(x => x.Permissions).HasMaxLength(128).IsRequired();
            builder.Property(x => x.Url).HasMaxLength(250);
            builder.Property(x => x.IsDisplay).HasDefaultValue(true);

            builder.HasIndex(x => x.ControllerName).IsUnique();

            builder
                .HasOne(x => x.Module)
                .WithMany(y => y.Functions)
                .HasForeignKey(x => x.ModuleId);
        }
    }
}