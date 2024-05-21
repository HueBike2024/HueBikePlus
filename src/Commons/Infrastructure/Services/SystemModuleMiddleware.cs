using Core.Extensions;
using Core.Helpers;
using Core.Models;
using Infrastructure.EF;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Serilog.Context;
using System.Net;


namespace Infrastructure.Services
{
    public class SystemModuleMiddleware
    {
        private readonly IServiceProvider _serviceProvider;
        public readonly RequestDelegate _next;
        public SystemModuleMiddleware(RequestDelegate next, IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _next = next;
        }
        public async Task Invoke(HttpContext context)
        {
            using var scope = _serviceProvider.CreateScope();
            using var dbContext = scope.ServiceProvider.GetRequiredService<BaseDbContext>();

            string path = $"{context.Request.Path}{context.Request.QueryString}";

            var systemModule = await dbContext.SystemModules.AsNoTracking().FirstOrDefaultAsync(e => path.StartsWith("/" + e.Segment + "/"));

            if (systemModule?.IsActive == false)
            {
                var json = new JsonResponse
                {
                    Message = systemModule.Message,
                    StatusCode = (int)HttpStatusCode.ServiceUnavailable
                };

                context.Response.StatusCode = (int)HttpStatusCode.ServiceUnavailable;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(JsonConvert.SerializeObject(json));

                return;
            }
            else
            {
                var responseBody = context.Response.Body;
                using var responseBodyStream = new MemoryStream();
                context.Response.Body = responseBodyStream;

                await _next(context);

                responseBodyStream.Seek(0, SeekOrigin.Begin);
                await responseBodyStream.CopyToAsync(responseBody);
                string method = context.Request.Method;
                string statusCode = context.Response.StatusCode.ToString();
                //string path = $"{context.Request.Scheme}://{context.Request.Host}{context.Request.Path}{context.Request.QueryString}";

                if (systemModule != null && systemModule.IsLog)
                {
                    LogContext.PushProperty("Path", path);
                    LogContext.PushProperty("Method", method);
                    LogContext.PushProperty("StatusCode", statusCode);
                    LogContext.PushProperty("UserName", TokenExtensions.GetUserName());

                    // Chỉ log những API có status nằm trong SYS.StatusCodes nếu SYS.StatusCodes != NULL/EMPTY
                    if (!string.IsNullOrEmpty(systemModule.StatusCodes))
                    {
                        List<int> statusCodes = new();
                        try
                        {
                            statusCodes = systemModule.StatusCodes.Split(',').Select(e => int.Parse(e)).ToList();
                        }
                        catch
                        {
                            return;
                        }

                        if (statusCodes.Any(e => e == context.Response.StatusCode) == false) return;
                    }

                    // SYS.StatusCodes = NULL/EMPTY thì log hết
                    responseBodyStream.Seek(0, SeekOrigin.Begin);
                    var responseBodyText = new StreamReader(responseBodyStream).ReadToEnd();

                    if (responseBodyText?.Length > 16380) responseBodyText.Truncate(16380);

                    if (context.Response.StatusCode == (int)HttpStatusCode.OK)
                    {
                        LogHelper.Logger.Information(responseBodyText);
                    }
                    else
                    {
                        LogHelper.Logger.Error(responseBodyText);
                    }
                }
            }
        }
    }
}
