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
    public class PaymentMessageConfiguration : BaseConfiguration, IEntityTypeConfiguration<PaymentMessage>
    {
        public void Configure(EntityTypeBuilder<PaymentMessage> builder)
        {
            builder.ToTable("PaymentMessage");
            builder.HasKey(x => new { x.Id });

            builder.Property(x => x.Name).HasMaxLength(200);
            builder.Property(x => x.Description).HasMaxLength(200);


            ConfigureBase(builder);
        }
    }
}
