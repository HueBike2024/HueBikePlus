using Infrastructure.AggregatesModel.Authen.AccountAggregate;
using Infrastructure.AggregatesModel.MasterData.ImageAggregate;
using Infrastructure.AggregatesModel.MasterData.NotificationAggregate;
using Infrastructure.AggregatesModel.MasterData.UserAggregate.ComplainAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.EntityConfigurations.MasterData.ImageConfig
{
    public class PictureConfiguration : BaseConfiguration, IEntityTypeConfiguration<Picture>
    {
        public void Configure(EntityTypeBuilder<Picture> builder)
        {
            builder.ToTable("Picture");
            builder.HasKey(x => new { x.Id });

            builder.Property(x => x.PublicId).HasMaxLength(120).IsRequired();
            builder.Property(x => x.NotificationId).HasMaxLength(20);
            builder.Property(x => x.ComplainId).HasMaxLength(20);
            builder.Property(x => x.ImageUrl).HasMaxLength(500).IsRequired();

            builder.HasIndex(x => x.PublicId).IsUnique();

            builder
                .HasOne(x => x.Notification)
                .WithOne(y => y.Picture)
                .HasForeignKey<Notification>(x => x.PictureId);

            builder
                .HasOne(x => x.Complain)
                .WithOne(y => y.Picture)
                .HasForeignKey<Complain>(x => x.PictureId);

            ConfigureBase(builder);
        }
    }
}
