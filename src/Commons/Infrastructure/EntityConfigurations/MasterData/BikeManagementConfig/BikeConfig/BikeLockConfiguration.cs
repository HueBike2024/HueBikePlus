using Infrastructure.AggregatesModel.Authen.AccountAggregate;
using Infrastructure.AggregatesModel.MasterData.BikeManagementAggregate.BikeAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.EntityConfigurations.MasterData.BikeManagementConfig.BikeConfig
{
    public class BikeLockConfiguration : BaseConfiguration, IEntityTypeConfiguration<BikeLock>
    {
        public void Configure(EntityTypeBuilder<BikeLock> builder)
        {
            builder.ToTable("BikeLock");
            builder.HasKey(x => new { x.Id });

            builder.Property(x => x.LockName).HasMaxLength(20).IsRequired();
            builder.Property(x => x.PathQr).HasMaxLength(200).IsRequired();
            builder.Property(x => x.Power).HasMaxLength(5).IsRequired();

            builder.HasIndex(x => x.PathQr).IsUnique();

            ConfigureBase(builder);
        }
    }
}
