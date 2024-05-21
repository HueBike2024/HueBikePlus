using Infrastructure.AggregatesModel.Authen.AccountAggregate;
using Infrastructure.AggregatesModel.MasterData.NotificationAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.EntityConfigurations.MasterData.NotificationConfig
{
    public class NotificationConfiguration : BaseConfiguration, IEntityTypeConfiguration<Notification>
    {
        public void Configure(EntityTypeBuilder<Notification> builder)
        {
            builder.ToTable("Notification");
            builder.HasKey(x => new { x.Id });

            builder.Property(x => x.Title).HasMaxLength(120).IsRequired();
            builder.Property(x => x.Image).HasMaxLength(500);
            builder.Property(x => x.Content).HasMaxLength(500).IsRequired();
            builder.Property(x => x.UserId).HasMaxLength(20);

            builder
                .HasMany(x => x.UserNotifications)
                .WithOne(y => y.Notification)
                .HasForeignKey(x => x.NotificationId);

            ConfigureBase(builder);
        }
    }
}
