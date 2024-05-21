using Infrastructure.AggregatesModel.Authen.AccountAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.EntityConfigurations.Authen.AccountConfig
{
    public class UserTokenConfiguration : IEntityTypeConfiguration<UserToken>
    {
        public void Configure(EntityTypeBuilder<UserToken> builder)
        {
            builder.ToTable("AUTH_UserToken");
            builder.HasKey(x => new { x.Id });

            builder.Property(x => x.Token).HasMaxLength(2000);
            builder.Property(x => x.IP).HasMaxLength(50);
            builder.Property(x => x.ExpiredTime).IsRequired();
            builder.Property(x => x.IsActive).HasDefaultValue(true);

            builder
                .HasOne(x => x.User)
                .WithMany(y => y.UserTokens)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
               .HasOne(x => x.RefreshToken)
               .WithMany(y => y.UserTokens)
               .HasForeignKey(x => x.RefreshTokenId)
               .OnDelete(DeleteBehavior.Restrict);
        }
    }
}