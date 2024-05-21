using Infrastructure.AggregatesModel.MasterData.TripManagementAggregate.RateAggregate;
using Infrastructure.AggregatesModel.MasterData.TripManagementAggregate.TripAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.EntityConfigurations.MasterData.TripManagementConfig.RateConfig
{
    public class RateConfiguration : BaseConfiguration, IEntityTypeConfiguration<Rate>
    {
        public void Configure(EntityTypeBuilder<Rate> builder)
        {
            builder.ToTable("Rate");
            builder.HasKey(x => new { x.Id });

            builder.Property(x => x.RateStar).HasMaxLength(1).IsRequired();
            builder.Property(x => x.Content).HasMaxLength(500).IsRequired();
            builder.Property(x => x.TripId).HasMaxLength(20).IsRequired();
            builder.Property(x => x.SenderId).HasMaxLength(20).IsRequired();

            builder.HasIndex(x => x.TripId).IsUnique();

            builder
                .HasOne(x => x.Trip)
                .WithOne(y => y.Rate)
                .HasForeignKey<Rate>(x => x.TripId);

            builder
                .HasOne(x => x.Sender)
                .WithMany(y => y.Rates)
                .HasForeignKey(x => x.SenderId);

            builder
                .HasMany(x => x.Replys)
                .WithOne(y => y.Rate)
                .HasForeignKey(x => x.RateId);

            ConfigureBase(builder);
        }
    }
}
