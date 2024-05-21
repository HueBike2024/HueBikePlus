using Infrastructure.AggregatesModel.MasterData.UnitAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.EntityConfigurations.MasterData.UnitConfig
{
    public class UnitConfiguration : BaseConfiguration, IEntityTypeConfiguration<Unit>
    {
        public void Configure(EntityTypeBuilder<Unit> builder)
        {
            builder.ToTable("MD_Unit");
            builder.HasKey(x => new { x.Id });

            builder.Property(x => x.Code).HasMaxLength(20).IsRequired();
            builder.Property(x => x.Name).HasMaxLength(250).IsRequired();
            builder.Property(x => x.Email).HasMaxLength(255);
            builder.Property(x => x.PhoneNumber).HasMaxLength(50);
            builder.Property(x => x.Fax).HasMaxLength(50);

            builder.HasIndex(x => x.Code).IsUnique();

            ConfigureBase(builder);
        }
    }
}
