using Infrastructure.AggregatesModel.Authen.AccountAggregate;
using Infrastructure.AggregatesModel.MasterData.TripManagementAggregate.TicketAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.EntityConfigurations.MasterData.TripManagementConfig.TicketConfig
{
    public class TicketConfiguration : BaseConfiguration, IEntityTypeConfiguration<Ticket>
    {
        public void Configure(EntityTypeBuilder<Ticket> builder)
        {
            builder.ToTable("Ticket");
            builder.HasKey(x => new { x.Id });

            builder.Property(x => x.TicketNo).HasMaxLength(20).IsRequired();
            builder.Property(x => x.UserFullName).HasMaxLength(50).IsRequired();
            builder.Property(x => x.UserPhone).HasMaxLength(20).IsRequired();
            builder.Property(x => x.PathQr).HasMaxLength(50);
            builder.Property(x => x.QrImage).HasMaxLength(500);
            builder.Property(x => x.BookingDate).HasMaxLength(50).IsRequired();
            builder.Property(x => x.ExpectedEndTime).HasMaxLength(50);
            builder.Property(x => x.ExpiryDate).HasMaxLength(50);
            builder.Property(x => x.CategoryTicketId).IsRequired();
            builder.Property(x => x.UserId).IsRequired();
            builder.Property(x => x.BikeId);
            builder.Property(x => x.StatusId).IsRequired();

            builder.HasIndex(x => x.TicketNo).IsUnique();
            builder.HasIndex(x => x.PathQr).IsUnique();

            builder
                .HasOne(x => x.User)
                .WithMany(y => y.Tickets)
                .HasForeignKey(x => x.UserId);

            builder
                .HasOne(x => x.Bike)
                .WithMany(y => y.Tickets)
                .HasForeignKey(x => x.BikeId);

            builder
                .HasOne(x => x.CategoryTicket)
                .WithMany(y => y.Tickets)
                .HasForeignKey(x => x.CategoryTicketId);

            builder
                .HasOne(x => x.Status)
                .WithMany(y => y.Tickets)
                .HasForeignKey(x => x.StatusId);

            ConfigureBase(builder);
        }
    }
}
