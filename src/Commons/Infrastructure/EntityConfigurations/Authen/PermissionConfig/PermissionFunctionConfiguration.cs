using Infrastructure.AggregatesModel.Authen.PermissionAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.EntityConfigurations.Authen.PermissionConfig
{
    public class PermissionFunctionConfiguration : IEntityTypeConfiguration<PermissionFunction>
    {
        public void Configure(EntityTypeBuilder<PermissionFunction> builder)
        {
            builder.ToTable("AUTH_PermissionFunction");
            builder.HasKey(x => new { x.PermissionId, x.FunctionId });

            builder.Property(x => x.Permissions).HasMaxLength(128).IsRequired();

            builder.HasIndex(x => x.PermissionId);
            builder.HasIndex(x => x.FunctionId);

            builder
                .HasOne(x => x.Permission)
                .WithMany(y => y.PermissionFunctions)
                .HasForeignKey(x => x.PermissionId);

            builder
                .HasOne(x => x.Function)
                .WithMany(y => y.PermissionFunctions)
                .HasForeignKey(x => x.FunctionId);
        }
    }
}