using Infrastructure.AggregatesModel.MasterData.UserAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.EntityConfigurations.MasterData.UserConfig
{
    public class UserPaymentConfiguration : BaseConfiguration, IEntityTypeConfiguration<UserPayment>
    {
        public void Configure(EntityTypeBuilder<UserPayment> builder)
        {
            builder.ToTable("UserPayment");
            builder.HasKey(x => new { x.Id });

            builder.Property(x => x.VpcAccessCode).HasMaxLength(200);
            builder.Property(x => x.VpcAmount).HasMaxLength(200);
            builder.Property(x => x.VpcMerchant).HasMaxLength(200);
            builder.Property(x => x.VpcMerchTxnRef).HasMaxLength(200);
            builder.Property(x => x.VpcSecureHash).HasMaxLength(200);
            builder.Property(x => x.VpcTicketNo).HasMaxLength(200);
            builder.Property(x => x.Price).HasMaxLength(20);
            builder.Property(x => x.Amount).HasMaxLength(200);
            builder.Property(x => x.Discount).HasMaxLength(20);
            builder.Property(x => x.TotalPrice).HasMaxLength(20);
            builder.Property(x => x.UserId).IsRequired().IsRequired();
            builder.Property(x => x.MessageId).HasMaxLength(20);
            builder.Property(x => x.Redirect).HasMaxLength(500);

            builder.HasIndex(x => x.VpcMerchTxnRef).IsUnique();

            builder
                .HasOne(x => x.User)
                .WithMany(y => y.PaymentHistorys)
                .HasForeignKey(x => x.UserId);

            ConfigureBase(builder);
        }
    }
}
