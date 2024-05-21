using Infrastructure.AggregatesModel.Authen.AccountAggregate;
using Infrastructure.AggregatesModel.MasterData.ImageAggregate;
using Infrastructure.AggregatesModel.MasterData.NotificationAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.EntityConfigurations.MasterData.ImageConfig
{
    public class AvatarConfiguration : BaseConfiguration, IEntityTypeConfiguration<Avatar>
    {
        public void Configure(EntityTypeBuilder<Avatar> builder)
        {
            builder.ToTable("Avatar");
            builder.HasKey(x => new { x.Id });

            builder.Property(x => x.PublicId).HasMaxLength(120).IsRequired();
            builder.Property(x => x.ImageUrl).HasMaxLength(500).IsRequired();

            builder.HasIndex(x => x.PublicId).IsUnique();

            builder
                .HasOne(x => x.User)
                .WithOne(y => y.ProfilePhoto)
                .HasForeignKey<User>(x => x.PhotoId);

            ConfigureBase(builder);
        }
    }
}
