using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;

namespace Authen.API.Configurations
{
    public static class SwaggerConfig
    {
        public static void Configure(IServiceCollection services, IConfiguration configuration)
        {
            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(option =>
            {
                option.SwaggerDoc("v1", new OpenApiInfo()
                {
                    Title = "BIKE-MANAGEMENT-AUTH",
                    Version = "v1",
                    Description = "Hệ thống quản lí xe đạp thông minh"
                });
                //option.OperationFilter<AddAuthorizationHeader>();
                option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });
                option.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            In = ParameterLocation.Header,

                        },
                        new List<string>()
                    }
                });

                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                option.IncludeXmlComments(xmlPath);
            });
        }

        public static void SwaggerMiddleware(IApplicationBuilder builder)
        {
            // Enable middleware to serve generated Swagger as a JSON endpoint
            builder.UseSwagger(c =>
            {
                c.SerializeAsV2 = true;
                c.RouteTemplate = "authen/{documentName}/swagger.json";
            });

            // Enable middleware to serve swagger-ui(HTML, JS, CSS, etc,),
            // specifying the Swagger JSON endpoint
            builder.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/authen/v1/swagger.json", "BASE-NET8 - Authentication");
                c.RoutePrefix = "authen";
                c.OAuthClientId("authen-base-net8");
                c.OAuthAppName("BASE-NET8 - Authentication Swagger UI");
            });
        }
    }

    public class AddAuthorizationHeader : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            operation.Parameters?.Add(new OpenApiParameter
            {
                Name = "Authorization",
                In = ParameterLocation.Header,
                Description = "Bearer \"{Token}\"",
                Required = false,
                Schema = new OpenApiSchema
                {
                    Type = "string"
                }
            });
        }
    }
}
