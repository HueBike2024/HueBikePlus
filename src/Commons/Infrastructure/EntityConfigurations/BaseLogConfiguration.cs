using Infrastructure.AggregatesModel.SystemLogs;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.EntityConfigurations
{
    public class BaseLogConfiguration
    {
        protected static void ConfigureBase<TEntity>(EntityTypeBuilder<TEntity> builder) where TEntity : BaseErrorLog
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("Id").ValueGeneratedOnAdd();
            builder.Property(x => x.Message).HasColumnName("Message").HasMaxLength(16383);
            builder.Property(x => x.MessageTemplate).HasColumnName("MessageTemplate").HasMaxLength(16383);
            builder.Property(x => x.Level).HasColumnName("Level").HasMaxLength(16383);
            builder.Property(x => x.TimeStamp).HasColumnName("TimeStamp");
            builder.Property(x => x.Exception).HasColumnName("Exception").HasMaxLength(16383);
            builder.Property(x => x.Path).HasMaxLength(250);
            builder.Property(x => x.Method).HasMaxLength(10);
            builder.Property(x => x.StatusCode).HasMaxLength(10);
            builder.Property(x => x.UserName).HasMaxLength(250);
        }
    }
}