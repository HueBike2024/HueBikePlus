using Infrastructure.AggregatesModel.MasterData.PostAggregate;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.AggregatesModel.MasterData.BannerAggregate;

namespace Infrastructure.EntityConfigurations.MasterData.BannerConfig
{
    public class BannerConfiguration : BaseConfiguration, IEntityTypeConfiguration<Banner>
    {
        public void Configure(EntityTypeBuilder<Banner> builder)
        {
            builder.ToTable("MD_Banner");
            builder.HasKey(x => new { x.Id });

            builder.Property(x => x.Title).HasMaxLength(255);
            builder.Property(x => x.Image).HasMaxLength(500);
            





            ConfigureBase(builder);
        }
    }
}
