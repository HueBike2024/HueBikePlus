using Infrastructure.AggregatesModel.MasterData.PostAggregate;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.AggregatesModel.MasterData.EventAggregate;

namespace Infrastructure.EntityConfigurations.MasterData.EventConfig
{
    public class EventConfiguration : BaseConfiguration, IEntityTypeConfiguration<Event>
    {
        public void Configure(EntityTypeBuilder<Event> builder)
        {
            builder.ToTable("MD_Event");
            builder.HasKey(x => new { x.Id });

            builder.Property(x => x.Title).HasMaxLength(255);
            builder.Property(x => x.Image).HasMaxLength(500);
            builder.Property(x => x.Description).HasMaxLength(5000);
            builder.Property(x =>x.DateStart).HasDefaultValue(DateTime.Now);
            builder.Property(x => x.DateEnd).HasDefaultValue(DateTime.Now);
            builder.Property(x => x.Location).HasMaxLength(500);
            builder.Property(x => x.Organizer).HasMaxLength(500);






            ConfigureBase(builder);
        }
    }
}
