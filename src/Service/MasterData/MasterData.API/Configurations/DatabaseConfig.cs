using Core.Interfaces.Database;
using Core.SeedWork.Repository;
using Infrastructure.EF;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using static Core.Common.Constants;

namespace MasterData.API.Configurations
{
    public class DatabaseConfig
    {
        public static void Configure(IServiceCollection services, IConfiguration configuration)
        {
            // MS SQL - Microsoft.EntityFrameworkCore.SqlServer
            services.AddEntityFrameworkSqlServer().AddDbContext<BaseDbContext>(options =>
            {
                //options.UseLazyLoadingProxies(); // auto get related entities of entity
                options.UseSqlServer(configuration.GetConnectionString(CONFIG_KEYS.APP_CONNECTION_STRING),
                    npgOption =>
                    {
                        npgOption.EnableRetryOnFailure(maxRetryCount: 15, maxRetryDelay: TimeSpan.FromSeconds(30), null);
                        npgOption.MigrationsAssembly(typeof(BaseDbContext).GetTypeInfo().Assembly.GetName().Name);
                        npgOption.CommandTimeout(3600);
                    });
                options.EnableSensitiveDataLogging(true);

            });

            // Inject IRepository
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            // Inject UnitOfWork
            services.AddScoped<IUnitOfWork, UnitOfWork>();
        }

        public static readonly ILoggerFactory ConsoleLoggerFactory = LoggerFactory.Create(builder =>
        {
            builder
            .AddFilter((category, level) =>
                category == DbLoggerCategory.Database.Command.Name && level == LogLevel.Debug)
            .AddConsole();
        });
    }
}
