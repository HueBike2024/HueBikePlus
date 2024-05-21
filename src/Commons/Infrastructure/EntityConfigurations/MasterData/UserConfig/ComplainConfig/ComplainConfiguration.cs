using Infrastructure.AggregatesModel.Authen.AccountAggregate;
using Infrastructure.AggregatesModel.MasterData.UserAggregate.AuthenticationAggregate;
using Infrastructure.AggregatesModel.MasterData.UserAggregate.ComplainAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.EntityConfigurations.MasterData.UserConfig.ComplainConfig
{
    public class ComplainConfiguration : BaseConfiguration, IEntityTypeConfiguration<Complain>
    {
        public void Configure(EntityTypeBuilder<Complain> builder)
        {
            builder.ToTable("Complain");
            builder.HasKey(x => new { x.Id });

            builder.Property(x => x.Content).HasMaxLength(500).IsRequired();
            builder.Property(x => x.Image).HasMaxLength(500);
            builder.Property(x => x.SenderId).HasMaxLength(20).IsRequired();


            builder
                .HasOne(x => x.ComplainSender)
                .WithMany(y => y.Complains)
                .HasForeignKey(x => x.SenderId);

            builder
                .HasMany(x => x.ComplainReplys)
                .WithOne(y => y.Complain)
                .HasForeignKey(x => x.ComplainId);


            ConfigureBase(builder);
        }
    }
}
