using Infrastructure.AggregatesModel.Authen.AccountAggregate;
using Infrastructure.AggregatesModel.MasterData.UserAggregate.ComplainAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.EntityConfigurations.MasterData.UserConfig.ComplainConfig
{
    public class ComplainReplyConfiguration : BaseConfiguration, IEntityTypeConfiguration<ComplainReply>
    {
        public void Configure(EntityTypeBuilder<ComplainReply> builder)
        {
            builder.ToTable("ComplainReply");
            builder.HasKey(x => new { x.Id });

            builder.Property(x => x.Content).HasMaxLength(500).IsRequired();
            builder.Property(x => x.ComplainId).HasMaxLength(20).IsRequired();
            builder.Property(x => x.SenderId).HasMaxLength(20).IsRequired();


            builder
                .HasOne(x => x.Complain)
                .WithMany(y => y.ComplainReplys)
                .HasForeignKey(x => x.ComplainId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne(x => x.Sender)
                .WithMany(y => y.ComplainReplys)
                .HasForeignKey(x => x.SenderId)
                .OnDelete(DeleteBehavior.Restrict);


            ConfigureBase(builder);
        }
    }
}