using Infrastructure.AggregatesModel.MasterData.TripManagementAggregate.RateAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.EntityConfigurations.MasterData.TripManagementConfig.RateConfig
{
    public class RateReplyConfiguration : BaseConfiguration, IEntityTypeConfiguration<RateReply>
    {
        public void Configure(EntityTypeBuilder<RateReply> builder)
        {
            builder.ToTable("RateReply");
            builder.HasKey(x => new { x.Id });

            builder.Property(x => x.Content).HasMaxLength(500).IsRequired();
            builder.Property(x => x.RateId).HasMaxLength(20).IsRequired();
            builder.Property(x => x.SenderId).HasMaxLength(20).IsRequired();

            builder
                .HasOne(x => x.Rate)
                .WithMany(y => y.Replys)
                .HasForeignKey(x => x.RateId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne(x => x.Sender)
                .WithMany(y => y.RateReplys)
                .HasForeignKey(x => x.SenderId)
                .OnDelete(DeleteBehavior.Restrict);


            ConfigureBase(builder);
        }
    }
}
