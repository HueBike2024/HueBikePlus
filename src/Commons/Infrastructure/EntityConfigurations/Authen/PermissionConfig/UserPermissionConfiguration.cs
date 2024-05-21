using Infrastructure.AggregatesModel.Authen.PermissionAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.EntityConfigurations.Authen.PermissionConfig
{
    public class UserPermissionConfiguration : IEntityTypeConfiguration<UserPermission>
    {
        public void Configure(EntityTypeBuilder<UserPermission> builder)
        {
            builder.ToTable("AUTH_UserPermission");
            builder.HasKey(x => new { x.PermissionId, x.UserId });

            builder.HasIndex(x => x.PermissionId);
            builder.HasIndex(x => x.UserId);

            builder
                .HasOne(x => x.Permission)
                .WithMany(y => y.UserPermissions)
                .HasForeignKey(x => x.PermissionId);

            builder
                .HasOne(x => x.User)
                .WithMany(y => y.UserPermissions)
                .HasForeignKey(x => x.UserId);
        }
    }
}