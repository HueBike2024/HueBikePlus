using Infrastructure.AggregatesModel.Authen.AccountAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.EntityConfigurations.Authen.AccountConfig
{
    public class UserConfiguration : BaseConfiguration, IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("AUTH_User");
            builder.HasKey(x => new { x.Id });

            builder.Property(x => x.FullName).HasMaxLength(125).IsRequired();
            builder.Property(x => x.UserName).HasMaxLength(50).IsRequired();
            builder.Property(x => x.Password).HasMaxLength(500).IsRequired();
            builder.Property(x => x.PhoneNumber).HasMaxLength(50);
            builder.Property(x => x.Email).HasMaxLength(255);
            builder.Property(x => x.Point).HasMaxLength(20);
            builder.Property(x => x.TimeZone).HasMaxLength(50);
            builder.Property(x => x.Address).HasMaxLength(255);
            builder.Property(x => x.IsLock).IsRequired();

            builder.HasIndex(x => x.UserName).IsUnique();

            builder
                .HasOne(x => x.Avatar)
                .WithOne(y => y.User)
                .HasForeignKey<User>(x => x.AvatarId);

            ConfigureBase(builder);
        }
    }
}