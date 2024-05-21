using Infrastructure.AggregatesModel.Authen;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.EntityConfigurations.Authen
{
    public class AuthenMediaConfiguration : BaseConfiguration, IEntityTypeConfiguration<AuthenMedia>
    {
        public void Configure(EntityTypeBuilder<AuthenMedia> builder)
        {
            builder.ToTable("AUTH_Media");

            ConfigureBase(builder);
        }
    }
}
