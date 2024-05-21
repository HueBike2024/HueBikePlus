using Infrastructure.AggregatesModel.Authen.AccountAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.EntityConfigurations.Authen.AccountConfig
{
    public class UserRefreshTokenConfiguration : IEntityTypeConfiguration<UserRefreshToken>
    {
        public void Configure(EntityTypeBuilder<UserRefreshToken> builder)
        {
            builder.ToTable("AUTH_UserRefreshToken");
            builder.HasKey(x => new { x.Id });

            builder.Property(x => x.RefreshToken).HasMaxLength(200);
            builder.Property(x => x.RefreshExpiredTime).IsRequired();
            builder.Property(x => x.IsActive).HasDefaultValue(true);

            builder
                .HasOne(x => x.User)
                .WithMany(y => y.UserRefreshTokens)
                .HasForeignKey(x => x.UserId);
        }
    }
}