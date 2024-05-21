using Infrastructure.AggregatesModel.System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.EntityConfigurations.System
{
    public class EmailServiceConfiguration : IEntityTypeConfiguration<EmailServer>
    {
        public void Configure(EntityTypeBuilder<EmailServer> builder)
        {
            builder.ToTable("SYS_EmailService");
            builder.HasKey(x => new { x.Id });

            builder.Property(x => x.Host).HasMaxLength(500);
            builder.Property(x => x.Account).HasMaxLength(200);
            builder.Property(x => x.Password).HasMaxLength(200);
            builder.Property(x => x.SenderName).HasMaxLength(500);
            builder.Property(x => x.EnableSsl).IsRequired().HasDefaultValue(true);
            builder.Property(x => x.DefaultCredentials).IsRequired().HasDefaultValue(false);
        }
    }
}
