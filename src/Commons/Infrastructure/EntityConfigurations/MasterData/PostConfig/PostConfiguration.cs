using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.AggregatesModel.MasterData.PostAggregate;

namespace Infrastructure.EntityConfigurations.MasterData.PostConfig
{
    public class PostConfiguration : BaseConfiguration, IEntityTypeConfiguration<Post>
    {
        public void Configure(EntityTypeBuilder<Post> builder)
        {
            builder.ToTable("MD_Post");
            builder.HasKey(x => new { x.Id });

            builder.Property(x => x.Title).HasMaxLength(255);
            builder.Property(x =>x.Image).HasMaxLength(500);
            builder.Property(x => x.Content).HasMaxLength(5000);
            
           

           

            ConfigureBase(builder);
        }
    }
}
