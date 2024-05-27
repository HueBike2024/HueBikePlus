using Autofac.Extensions.DependencyInjection;
using CloudinaryDotNet;
using Core.Attributes;
using Core.Extensions;
using Core.Helpers;
using Core.Implements.Http;
using Core.Middleware;
using Core.Models.Base;
using Core.Models.Settings;
using FluentValidation;
using FluentValidation.AspNetCore;
using Hangfire;
using Hangfire.SqlServer;
using Infrastructure.AggregatesModel.MasterData.PaymentConst;
using Infrastructure.EF;
using Infrastructure.EntityConfigurations.MasterData.LogConfig;
using Infrastructure.Services;
using MasterData.API.Configurations;
using MasterData.Application.Extentions;
using MasterData.Application.Services.CloudinaryService;
using MasterData.Application.Services.GoogleMaps;
using MasterData.Application.Services.TicketService;
using MasterData.Application.Services.TripService;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.FileProviders;
using SixLabors.ImageSharp;

var builder = WebApplication.CreateBuilder(args);

IWebHostEnvironment environment = builder.Environment;
Console.WriteLine($"Enviroment name: {environment.EnvironmentName}");

var configuration = ConfigurationHelper.GetConfiguration(builder.Configuration);

// Authen LOG
//LogHelper.ErrorLogger = LogHelper.CreateErrorLogger(configuration, $"\"{AuthenticationErrorLogConfiguration.TABLE_NAME}\"");
LogHelper.Logger = LogHelper.CreateLogger(configuration, MasterDataLogConfiguration.TABLE_NAME);
// --

TimeZoneInfo serverTimeZone = TimeZoneInfo.Local;
Console.WriteLine("Server time zone: " + serverTimeZone.DisplayName);

var services = builder.Services;

var appSettingsSection = builder.Configuration.GetSection("AppSettings");
services.Configure<AppSettings>(appSettingsSection);
var appSettings = appSettingsSection.Get<AppSettings>();

// Đăng ký Cloudinary và cấu hình
Account account = new Account(
    "dodlxvygb",
    "738694158418772",
    "3yJbyxj9koUtAdxp90zqYCn0FSI");
Cloudinary cloudinary = new Cloudinary(account);
services.AddSingleton(cloudinary);

//Đăng kí dịch vụ payment
services.Configure<PaymentServiceOptions>(builder.Configuration.GetSection("PaymentServiceOptions"));

//Đăng kí random
builder.Services.AddSingleton<IRandomService, RandomStringService>();

//Đăng kí dịch vụ map
builder.Services.AddHttpClient();
builder.Services.AddSingleton<IMapService, MapService>();

// Đăng ký service ImageUploader
services.AddSingleton<ICloudPhotoService, CloudPhotoService>();

//Dịch vụ kiểm tra vé
//services.AddHostedService<CheckTicketTimeBefore15M>();
//services.AddHostedService<CheckTicketAvailability>();

// Add services to the container.
services.AddControllers();

// Using a custom DI container.
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

// Cấu hình bổ sung
services.AddRazorPages();

services.AddMemoryCache();
services.AddDirectoryBrowser();
services.AddAuthentication().AddCookie(opts =>
{
    opts.Cookie.HttpOnly = true;
});

services.AddCors();

//configure cookie
services.ConfigureApplicationCookie(options =>
{
    // Cookie settings
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromMinutes(5);
    options.SlidingExpiration = true;
});

// Cấu hình Hangfire
builder.Services.AddHangfire(configuration => configuration
    .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
    .UseSimpleAssemblyNameTypeSerializer()
    .UseRecommendedSerializerSettings()
    .UseSqlServerStorage(builder.Configuration.GetConnectionString("MainDatabase"), new SqlServerStorageOptions
    {
        CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
        SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
        QueuePollInterval = TimeSpan.Zero,
        UseRecommendedIsolationLevel = true,
        UsePageLocksOnDequeue = true,
        DisableGlobalLocks = true
    }));

builder.Services.AddHangfireServer();

services.AddHttpContextAccessor();
//Lower case api url
services.AddRouting(options => { options.LowercaseUrls = true; });

services.AddSingleton(appSettings);
services.AddSingleton(new AppModule { Name = "24-BASE-MasterData" });

services.AddScoped<ICloudPhotoService, CloudPhotoService>();

// Common
CommonConfig.Configure(services, builder.Configuration);

// Swagger
SwaggerConfig.Configure(services, builder.Configuration);

// JWT
JwtConfig.Configure(services, builder.Configuration);

// Database
// Database
DatabaseConfig.Configure(services, builder.Configuration);

// Rate Limit Service
RateLimitServiceConfig.Configure(services, builder.Configuration);

// Global filter
services.AddMvc(options =>
{
    options.Filters.Add(typeof(HttpGlobalExceptionFilter));
});

services.AddFluentValidationAutoValidation();
services.AddFluentValidationClientsideAdapters();
services.AddValidatorsFromAssemblyContaining<Program>();

services.AddFluentValidationClientsideAdapters();
services.AddHealthChecks();

// CORS
builder.Services.AddCors(p => p.AddPolicy("MyCors", build =>
{


    build.WithOrigins("*").AllowAnyMethod().AllowAnyHeader();
}));

// Migration database - 1
builder.Services.AddDbContext<BaseDbContext>();

services.AddControllers(config =>
{
    config.Filters.Add(new ValidateModelAttribute());
}).AddControllersAsServices().AddNewtonsoftJson();

// APP configuration
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
});

app.UseCors(options => options.WithOrigins(appSettings?.AllowedHosts).AllowAnyMethod().AllowAnyHeader().AllowCredentials());

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseStaticFiles(new StaticFileOptions()
{
    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"Media")),
    RequestPath = new PathString("/Media")
});
app.UseRouting();

app.UseCors("MyCors");
app.UseCors(x => x
    .AllowAnyMethod()
    .AllowAnyHeader()
    .SetIsOriginAllowed(origin => true) // allow any origin 
    .AllowCredentials());

app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto,
    ForwardLimit = 1
});
app.UseCookiePolicy();

app.UseAuthentication();
app.UseAuthorization();

//Module
app.UseMiddleware<SystemModuleMiddleware>();

app.UseEndpoints(endpoints =>
{
    _ = endpoints.MapControllers();
});

HttpAppContext.Configure(app.Services.GetRequiredService<IHttpContextAccessor>());
HttpAppService.Configure(app.Services);

// Swagger Middleware
SwaggerConfig.SwaggerMiddleware(app);

// Jwt Token 
TokenExtensions.Configure(app.Services.GetRequiredService<IHttpContextAccessor>());

// Migration database - 2
app.MigrateDbContext<BaseDbContext>((context, services) =>
{
    // Seeder data
});

// Sử dụng Hangfire Dashboard
app.UseHangfireDashboard();

// Sử dụng Hangfire Server
app.UseHangfireServer();

// Cấu hình các công việc sẽ được thực thi
RecurringJob.AddOrUpdate<ITicketService>("CheckTicketTime", x => x.CheckTicketTime(CancellationToken.None), Cron.MinuteInterval(1));
RecurringJob.AddOrUpdate<ITripService>("CheckTrips", x => x.CheckTrips(), Cron.MinuteInterval(1));

app.MapRazorPages();
app.Run();
