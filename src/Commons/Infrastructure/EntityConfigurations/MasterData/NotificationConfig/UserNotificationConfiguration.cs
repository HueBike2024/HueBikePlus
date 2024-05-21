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
    public class UserNotificationConfiguration : BaseConfiguration, IEntityTypeConfiguration<UserNotification>
    {
        public void Configure(EntityTypeBuilder<UserNotification> builder)
        {
            builder.ToTable("UserNotification");
            builder.HasKey(x => new { x.Id });

            builder.Property(x => x.UserId).HasMaxLength(20).IsRequired();
            builder.Property(x => x.NotificationId).HasMaxLength(20);
            builder.Property(x => x.IsRead).HasMaxLength(20).IsRequired();

            builder
                .HasOne(x => x.Notification)
                .WithMany(y => y.UserNotifications)
                .HasForeignKey(x => x.NotificationId);

            builder
                .HasOne(x => x.User)
                .WithMany(y => y.UserNotifications)
                .HasForeignKey(x => x.UserId);

            ConfigureBase(builder);
        }
    }
}
