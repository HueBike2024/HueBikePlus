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
    public class BikeConfiguration : BaseConfiguration, IEntityTypeConfiguration<Bike>
    {
        public void Configure(EntityTypeBuilder<Bike> builder)
        {
            builder.ToTable("Bike");
            builder.HasKey(x => new { x.Id });

            builder.Property(x => x.BikeName).HasMaxLength(50).IsRequired();
            builder.Property(x => x.PathQr).HasMaxLength(50);
            builder.Property(x => x.QrCodeImage).HasMaxLength(500);
            builder.Property(x => x.Power).HasMaxLength(3);
            builder.Property(x => x.RentalQuantity).HasMaxLength(10);
            builder.Property(x => x.StatusId).HasMaxLength(20).IsRequired(); 
            builder.Property(x => x.StationId).HasMaxLength(20).IsRequired();

            builder
                .HasOne(x => x.Status)
                .WithMany(y => y.Bikes)
                .HasForeignKey(x => x.StatusId)
                .OnDelete(DeleteBehavior.Restrict); 

            builder
                .HasOne(x => x.Station)
                .WithMany(y => y.Bikes)
                .HasForeignKey(x => x.StationId)
                .OnDelete(DeleteBehavior.Restrict); 

            ConfigureBase(builder);
        }
    }
}
