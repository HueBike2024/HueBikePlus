using Infrastructure.AggregatesModel.Authen.AccountAggregate;
using Infrastructure.AggregatesModel.MasterData.UserAggregate.AuthenticationAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.EntityConfigurations.MasterData.UserConfig.UserAuthenticationConfig
{
    public class UserAuthenticationConfiguration : BaseConfiguration, IEntityTypeConfiguration<UserAuthentication>
    {
        public void Configure(EntityTypeBuilder<UserAuthentication> builder)
        {
            builder.ToTable("UserAuthentication");
            builder.HasKey(x => new { x.Id });

            builder.Property(x => x.CardId).HasMaxLength(20).IsRequired();
            builder.Property(x => x.IssueDate).IsRequired();
            builder.Property(x => x.ExpiryDate).IsRequired();
            builder.Property(x => x.UserId).IsRequired();

            builder.HasIndex(x => x.CardId).IsUnique();
            builder.HasIndex(x => x.UserId).IsUnique();

            ConfigureBase(builder);
        }
    }
}
