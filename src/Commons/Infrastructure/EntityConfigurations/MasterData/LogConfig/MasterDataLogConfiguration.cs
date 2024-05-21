using Infrastructure.AggregatesModel.MasterData.LogAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.EntityConfigurations.MasterData.LogConfig
{
    public class MasterDataLogConfiguration : BaseLogConfiguration, IEntityTypeConfiguration<MasterDataLog>
    {
        public const string TABLE_NAME = "MD_Log";
        public void Configure(EntityTypeBuilder<MasterDataLog> builder)
        {
            builder.ToTable(TABLE_NAME);
            ConfigureBase(builder);
        }
    }
}
