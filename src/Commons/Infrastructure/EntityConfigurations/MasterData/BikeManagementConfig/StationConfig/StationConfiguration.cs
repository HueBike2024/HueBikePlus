using Infrastructure.AggregatesModel.Authen.AccountAggregate;
using Infrastructure.AggregatesModel.MasterData.BikeManagementAggregate.StationAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.EntityConfigurations.MasterData.BikeManagementConfig.StationConfig
{
    public class StationConfiguration : BaseConfiguration, IEntityTypeConfiguration<Station>
    {
        public void Configure(EntityTypeBuilder<Station> builder)
        {
            builder.ToTable("Station");
            builder.HasKey(x => new { x.Id });

            builder.Property(x => x.StationName).HasMaxLength(50).IsRequired();
            builder.Property(x => x.QuantityAvaiable).HasMaxLength(10).IsRequired();
            builder.Property(x => x.NumOfSeats).HasMaxLength(10).IsRequired();
           
            builder.Property(x => x.StatusId).HasMaxLength(255).IsRequired();

           

           

            builder
                .HasOne(x => x.Status)
                .WithMany(y => y.Stations)
                .HasForeignKey(x => x.StatusId);

            ConfigureBase(builder);
        }
    }
}
