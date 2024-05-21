using Core.Implements.Html;
using Core.Implements.Jwt;
using Core.Implements.Logging;
using Core.Implements.Pdf;
using Core.Interfaces.Html;
using Core.Interfaces.Jwt;
using Core.Interfaces.Logging;
using Core.Interfaces.Pdf;
using Core.Models.Settings;
using DinkToPdf;
using DinkToPdf.Contracts;
using Infrastructure.Services;
using NetCore.AutoRegisterDi;
using System.Reflection;
using System.Text;
using static Core.Common.Constants;

namespace Authen.API.Configurations
{
    public static class CommonConfig
    {
        public static void Configure(IServiceCollection services, IConfiguration configuration)
        {
            services.AddOptions();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            // Register code
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            // Inject Services
            services
            .RegisterAssemblyPublicNonGenericClasses(DIAssemblies.AssembliesToScan)
                .Where(t => t.Name.EndsWith("Service"))
                .AsPublicImplementedInterfaces(ServiceLifetime.Scoped);

            // Inject Handlers
            services
            .RegisterAssemblyPublicNonGenericClasses(DIAssemblies.AssembliesToScan)
                .Where(t => t.Name.EndsWith("Handler"))
                .AsPublicImplementedInterfaces(ServiceLifetime.Scoped);

            // Inject Helpers
            services
            .RegisterAssemblyPublicNonGenericClasses(DIAssemblies.AssembliesToScan)
                .Where(t => t.Name.EndsWith("Helper"))
                .AsPublicImplementedInterfaces(ServiceLifetime.Scoped);

            // Inject Query
            services
            .RegisterAssemblyPublicNonGenericClasses(DIAssemblies.AssembliesToScan)
                .Where(t => t.Name.EndsWith("Query"))
                .AsPublicImplementedInterfaces(ServiceLifetime.Scoped);

            // Jwt
            services.AddScoped<IJwtProvider, JwtProvider>();
            services.AddScoped<IJwtGenerator, JwtGenerator>();

            // Inject AutoMapper
            services.AddAutoMapper(DIAssemblies.AssembliesToScan);

            // Inject MediatR
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(DIAssemblies.AssembliesToScan));

            // Inject Loggers
            services.AddSingleton<IAppLogger, AppLogger>();

            // Inject Email
            services.AddSingleton<IEmailService, EmailService>();

            // Inject HtmlTemplate
            services.AddScoped<ITemplateService, TemplateService>();

            // Inject Pdf
            services.AddSingleton<IConverter>(new SynchronizedConverter(new PdfTools()));
            services.AddScoped<IPdfService, PdfService>();

            // Inject HttpClientFactory
            services.AddScoped<Core.Infrastructure.Factory.IHttpClientFactory, Core.Infrastructure.Factory.HttpClientFactory>();

            // Media
            services.Configure<MediaSetting>(configuration.GetSection(CONFIG_KEYS.MEDIA_SETTING));

            //var mediaSetting = new MediaSetting();
            //configuration.GetSection(CONFIG_KEYS.MEDIA_SETTING).Bind(mediaSetting);

            //services.Configure<FormOptions>(o =>
            //{
            //    o.ValueLengthLimit = int.MaxValue;
            //    o.MultipartBodyLengthLimit = mediaSetting.MaxFileSize;
            //    o.MemoryBufferThreshold = int.MaxValue;
            //});
        }
    }
}
