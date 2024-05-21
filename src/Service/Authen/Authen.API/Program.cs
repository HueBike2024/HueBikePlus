using Authen.API.Configurations;
using Autofac.Extensions.DependencyInjection;
using Core.Attributes;
using Core.Extensions;
using Core.Helpers;
using Core.Implements.Http;
using Core.Models.Base;
using Core.Models.Settings;
using FluentValidation;
using FluentValidation.AspNetCore;
using Infrastructure.EF;
using Infrastructure.EntityConfigurations.Authen.LogConfig;
using Infrastructure.Seeders;
using Infrastructure.Services;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

IWebHostEnvironment environment = builder.Environment;
Console.WriteLine($"Enviroment name: {environment.EnvironmentName}");

var configuration = ConfigurationHelper.GetConfiguration(builder.Configuration);

// Authen LOG
//LogHelper.ErrorLogger = LogHelper.CreateErrorLogger(configuration, $"\"{AuthenticationErrorLogConfiguration.TABLE_NAME}\"");
LogHelper.Logger = LogHelper.CreateLogger(configuration, AuthenticationLogConfiguration.TABLE_NAME);
// --

var services = builder.Services;

var appSettingsSection = builder.Configuration.GetSection("AppSettings");
services.Configure<AppSettings>(appSettingsSection);
var appSettings = appSettingsSection.Get<AppSettings>();

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

services.AddHttpContextAccessor();
//Lower case api url
services.AddRouting(options => { options.LowercaseUrls = true; });

services.AddSingleton(appSettings);
services.AddSingleton(new AppModule { Name = "BASE-NET8-Authen" });

// Common
CommonConfig.Configure(services, builder.Configuration);

// Swagger
SwaggerConfig.Configure(services, builder.Configuration);

// JWT
JwtConfig.Configure(services, builder.Configuration);

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

using (var scpoe = app.Services.CreateScope())
{
    var dataContext = scpoe.ServiceProvider.GetRequiredService<BaseDbContext>();
    dataContext.Database.Migrate();

    AuthSeeder.SeedAsync(dataContext).Wait();
}

app.MapRazorPages();
app.Run();
