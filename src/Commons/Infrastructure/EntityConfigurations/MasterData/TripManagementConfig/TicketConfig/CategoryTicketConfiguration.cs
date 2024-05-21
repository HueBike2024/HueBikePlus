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
    public class CategoryTicketConfiguration : BaseConfiguration, IEntityTypeConfiguration<CategoryTicket>
    {
        public void Configure(EntityTypeBuilder<CategoryTicket> builder)
        {
            builder.ToTable("CategoryTicket");
            builder.HasKey(x => new { x.Id });

            builder.Property(x => x.CategoryTicketName).HasMaxLength(50).IsRequired();
            builder.Property(x => x.Description).HasMaxLength(500);
            builder.Property(x => x.UserTime).HasMaxLength(20).IsRequired();
            builder.Property(x => x.Price).HasMaxLength(20).IsRequired();

            builder
                .HasMany(x => x.Tickets)
                .WithOne(y => y.CategoryTicket)
                .HasForeignKey(x => x.CategoryTicketId);

            ConfigureBase(builder);
        }
    }
}
