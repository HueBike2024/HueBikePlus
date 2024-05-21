using Core.Implements.Jwt;
using Core.Interfaces.Jwt;
using Core.Models.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using static Core.Common.Constants;

namespace Authen.API.Configurations
{
    public static class JwtConfig
    {
        public static IServiceCollection Configure(this IServiceCollection services, IConfiguration configuration)
        {
            // Config JWT
            services.Configure<JwtTokenSetting>(configuration.GetSection(CONFIG_KEYS.JWT_TOKEN));
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });
            services.AddScoped<IJwtProvider, JwtProvider>();
            services.AddScoped<IJwtGenerator, JwtGenerator>();

            // ===== Add Jwt Authentication ========
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear(); // remove default claims

            var appSettings = configuration.GetSection("AppSettings").Get<AppSettings>();
            services.AddAuthentication(o =>
            {
                o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                o.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(o =>
            {
                o.IncludeErrorDetails = true;
                o.RefreshOnIssuerKeyNotFound = true;
                o.SaveToken = true;
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = appSettings.Jwt.Issuer,
                    ValidAudience = appSettings.Jwt.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(appSettings.Jwt.SecretKey)),
                    ClockSkew = TimeSpan.Zero,
                };
            });
            services.Configure<SecurityStampValidatorOptions>(options =>
            {
                options.ValidationInterval = TimeSpan.FromMinutes(1);
            });
            return services;
        }
    }
}
