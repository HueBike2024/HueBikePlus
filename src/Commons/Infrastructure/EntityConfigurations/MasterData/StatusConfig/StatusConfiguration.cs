using Infrastructure.AggregatesModel.Authen.AccountAggregate;
using Infrastructure.AggregatesModel.MasterData.StatusAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.EntityConfigurations.MasterData.StatusConfig
{
    public class StatusConfiguration : BaseConfiguration, IEntityTypeConfiguration<Status>
    {
        public void Configure(EntityTypeBuilder<Status> builder)
        {
            builder.ToTable("Status");
            builder.HasKey(x => new { x.Id });

            builder.Property(x => x.StatusName).HasMaxLength(50).IsRequired();


            builder
                .HasMany(x => x.Users)
                .WithOne(y => y.Status)
                .HasForeignKey(x => x.StatusId);

            builder
                .HasMany(x => x.Bikes)
                .WithOne(y => y.Status)
                .HasForeignKey(x => x.StatusId);

            builder
                .HasMany(x => x.Stations)
                .WithOne(y => y.Status)
                .HasForeignKey(x => x.StatusId);

            ConfigureBase(builder);
        }
    }
}
