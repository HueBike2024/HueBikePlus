using Infrastructure.AggregatesModel.MasterData.UserAggregate;
using Infrastructure.AggregatesModel.MasterData.UserAggregate.AuthenticationAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.EntityConfigurations.MasterData.UserConfig
{
    public class TransactionConfiguration : BaseConfiguration, IEntityTypeConfiguration<Transaction>
    {
        public void Configure(EntityTypeBuilder<Transaction> builder)
        {
            builder.ToTable("_Transaction");
            builder.HasKey(x => new { x.Id });
            builder.Property(x => x.TransactionType).HasMaxLength(100);
            builder.Property(x => x.TransactionCode).HasMaxLength(30);
            builder.Property(x => x.Point).HasMaxLength(20).IsRequired();
            builder.Property(x => x.UserId).IsRequired();
            builder.Property(x => x.IsSuccess).IsRequired();

            builder.HasIndex(x => x.TransactionCode).IsUnique();

            builder
                .HasOne(x => x.User)
                .WithMany(y => y.Transactions)
                .HasForeignKey(x => x.UserId);

            ConfigureBase(builder);
        }
    }
}
