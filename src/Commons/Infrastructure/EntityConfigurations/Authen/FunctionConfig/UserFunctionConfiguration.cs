using Infrastructure.AggregatesModel.Authen.FunctionAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.EntityConfigurations.Authen.FunctionConfig
{
    public class UserFunctionConfiguration : IEntityTypeConfiguration<UserFunction>
    {
        public void Configure(EntityTypeBuilder<UserFunction> builder)
        {
            builder.ToTable("AUTH_UserFunction");
            builder.HasKey(x => new { x.UserId, x.FunctionId });

            builder.Property(x => x.Permissions).HasMaxLength(128).IsRequired();

            builder.HasIndex(x => x.UserId);
            builder.HasIndex(x => x.FunctionId);

            builder
                .HasOne(x => x.User)
                .WithMany(y => y.UserFunctions)
                .HasForeignKey(x => x.UserId);

            builder
                .HasOne(x => x.Function)
                .WithMany(y => y.UserFunctions)
                .HasForeignKey(x => x.FunctionId);
        }
    }
}