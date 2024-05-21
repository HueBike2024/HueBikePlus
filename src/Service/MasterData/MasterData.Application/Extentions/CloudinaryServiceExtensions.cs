using CloudinaryDotNet;
using MasterData.Application.Services.CloudinaryService;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterData.Application.Extentions
{
    public static class CloudinaryServiceExtensions
    {
        public static IServiceCollection AddCloudinaryServices(this IServiceCollection services)
        {
            var cloudName = Environment.GetEnvironmentVariable("CLOUDINARY_CLOUD_NAME") ?? "AccountName";
            var apiKey = Environment.GetEnvironmentVariable("CLOUDINARY_API_KEY") ?? "";
            var apiSecret = Environment.GetEnvironmentVariable("CLOUDINARY_API_SECRET") ?? "";

            var account = new Account(cloudName, apiKey, apiSecret);

            var cloudinary = new Cloudinary(account);
            cloudinary.Api.Secure = true;

            services.AddSingleton(cloudinary);
            services.AddScoped<ICloudPhotoService, CloudPhotoService>();

            return services;
        }
    }
}
