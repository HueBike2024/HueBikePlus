using Infrastructure.AggregatesModel.Authen.AccountAggregate;
using Infrastructure.AggregatesModel.MasterData.BikeManagementAggregate.MapLocationAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.EntityConfigurations.MasterData.BikeManagementConfig.MapLocationConfig
{
    public class MapLocationConfiguration : BaseConfiguration, IEntityTypeConfiguration<MapLocation>
    {
        public void Configure(EntityTypeBuilder<MapLocation> builder)
        {
            builder.ToTable("MapLocation");
            builder.HasKey(x => new { x.Id });

            builder.Property(x => x.LocationName).HasMaxLength(50).IsRequired();
            builder.Property(x => x.Longitude).HasMaxLength(200).IsRequired();
            builder.Property(x => x.Latitude).HasMaxLength(200).IsRequired();

            ConfigureBase(builder);
        }
    }
}
