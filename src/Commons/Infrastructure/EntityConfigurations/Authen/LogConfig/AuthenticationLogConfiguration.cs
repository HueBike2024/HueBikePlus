using Infrastructure.AggregatesModel.Authen.LogAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.EntityConfigurations.Authen.LogConfig
{
    public class AuthenticationLogConfiguration : BaseLogConfiguration, IEntityTypeConfiguration<AuthenticationLog>
    {
        public const string TABLE_NAME = "AUTH_Log";
        public void Configure(EntityTypeBuilder<AuthenticationLog> builder)
        {
            builder.ToTable(TABLE_NAME);
            ConfigureBase(builder);
        }
    }
}
