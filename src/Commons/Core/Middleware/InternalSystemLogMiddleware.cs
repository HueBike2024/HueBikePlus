using Core.Extensions;
using Core.Helpers;
using Core.Models.Settings;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Serilog.Context;

namespace Core.Middleware
{
    public class InternalSystemLogMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _configuration;

        public InternalSystemLogMiddleware(RequestDelegate next, IConfiguration configuration)
        {
            _next = next;
            _configuration = configuration;
        }

        public async Task Invoke(HttpContext context)
        {
            #region Các trường hợp không cần ghi log
            if (context.Request.Host.ToString().Contains("localhost"))
            {
                await _next(context);
                return;
            }
            if (context.Request.Path.ToString().Contains("index.html"))
            {
                await _next(context);
                return;
            }
            if (context.Request.Path.ToString().Contains("swagger.json"))
            {
                await _next(context);
                return;
            }
            #endregion

            // Request body
            using var requestBodyStream = new MemoryStream();
            await context.Request.Body.CopyToAsync(requestBodyStream);
            requestBodyStream.Seek(0, SeekOrigin.Begin);
            var requestBodyText = new StreamReader(requestBodyStream).ReadToEnd();
            var requestBodyString = $"{context.Request.Method} {context.Request.Scheme}://{context.Request.Host}{context.Request.Path}{context.Request.QueryString} {requestBodyText}";
            requestBodyStream.Seek(0, SeekOrigin.Begin);
            context.Request.Body = requestBodyStream;

            // Response body
            var responseBody = context.Response.Body;
            using var responseBodyStream = new MemoryStream();
            context.Response.Body = responseBodyStream;

            await _next(context);

            responseBodyStream.Seek(0, SeekOrigin.Begin);
            var responseBodyText = new StreamReader(responseBodyStream).ReadToEnd();
            var responseBodyString = $"{context.Response.StatusCode}: {responseBodyText}";
            responseBodyStream.Seek(0, SeekOrigin.Begin);
            await responseBodyStream.CopyToAsync(responseBody);

            // TODO: maybe disable and use other
            //LogHelper.InternalSystemLogger.Information("REQUEST: {0} \nRESPONSE:{1}", requestBodyString, responseBodyString);

            if (!IsAllowLog(context.Request, context.Response))
            {
                return;
            }

            // ---
            string method = context.Request.Method;
            string statusCode = context.Response.StatusCode.ToString();
            string path = $"{context.Request.Scheme}://{context.Request.Host}{context.Request.Path}{context.Request.QueryString}";
            string authorization = context.Request.Headers.ContainsKey("Authorization") ? context.Request.Headers["Authorization"] : "";
            string userAgent = context.Request.Headers.ContainsKey("User-Agent") ? context.Request.Headers["User-Agent"] : "";

            var username = TokenExtensions.GetUserName() ?? "";
            _ = Task.Factory.StartNew(() =>
            {
                LogContext.PushProperty("Username", username ?? "");
                if (!string.IsNullOrEmpty(username))
                {
                    LogHelper.UserActionLogger.Information("");
                }
                LogContext.PushProperty("Method", method ?? "");
                LogContext.PushProperty("StatusCode", statusCode ?? "");
                LogContext.PushProperty("Path", path ?? "");
                LogContext.PushProperty("Authorization", authorization ?? "");
                LogContext.PushProperty("UserAgent", userAgent ?? "");

                if ((requestBodyText == null || requestBodyText.Length <= 16383) && (responseBodyText == null || responseBodyText.Length <= 16383))
                {
                    LogContext.PushProperty("RequestBody", requestBodyText ?? "");
                    LogContext.PushProperty("ResponseBody", responseBodyText ?? "");
                    LogContext.PushProperty("LogUrl", "");
                }
                else
                {
                    LogContext.PushProperty("RequestBody", requestBodyText?.Truncate(16380) ?? "");
                    LogContext.PushProperty("ResponseBody", responseBodyText?.Truncate(16380) ?? "");

                    //// Xử lý khi có Upload File
                    //var enableLogFileMinIO = _configuration.GetValue<bool?>("EnableLogFileMinIO") ?? true;
                    //if (enableLogFileMinIO)
                    //{
                    //    var logFile = $"logs/internal/{DateTime.UtcNow.ToLocalTime():yyyy/MM/dd}/{Guid.NewGuid()}.txt";
                    //    var minIOSetting = _configuration.GetSection(CONFIG_KEYS.MINIO).Get<MinIOSetting>();
                    //    var logUrl = minIOSetting.PublicUrl + "/" + minIOSetting.BucketName + "/" + logFile;
                    //    LogContext.PushProperty("LogUrl", logUrl);
                    //    try
                    //    {
                    //        using (var ms = new MemoryStream())
                    //        {
                    //            using (TextWriter tw = new StreamWriter(ms, Encoding.UTF8))
                    //            {
                    //                tw.WriteLine($"LogUrl: {logUrl}");
                    //                tw.WriteLine($"Method: {method}");
                    //                tw.WriteLine($"StatusCode: {statusCode}");
                    //                tw.WriteLine($"Path: {path}");
                    //                tw.WriteLine($"Authorization: {authorization}");
                    //                tw.WriteLine($"RequestBody: {requestBodyText}");
                    //                tw.WriteLine($"ResponseBody: {responseBodyText}");
                    //                tw.WriteLine($"UserAgent: {userAgent}");
                    //                tw.WriteLine($"UserName: {username}");
                    //                tw.Flush();
                    //                ms.Position = 0;
                    //                //await uploadFileService.Upload(ms, logFile);
                    //            }
                    //        }
                    //    }
                    //    catch (Exception ex) { }
                    //}
                }

                LogHelper.InternalLogger.Information("");
            });
        }

        private bool IsAllowLog(HttpRequest request, HttpResponse response)
        {
            var settings = _configuration.GetSection(nameof(InternalLogSetting)).Get<InternalLogSetting>();
            if (!settings.Enable)
            {
                return false;
            }
            if (!settings.LogSettings.Any())
            {
                return true;
            }

            var method = request.Method.ToLower();
            var path = request.Path.ToString();
            var headers = request.Headers.Keys.Select(x => x.ToLower());
            var statusCode = response.StatusCode;

            foreach (var setting in settings.LogSettings)
            {
                if (setting.DenyHeaders.Exists(x => headers.Contains(x.ToLower())))
                {
                    continue;
                }

                if (setting.AllowHeaders.Any() && setting.AllowHeaders.Exists(x => !headers.Contains(x.ToLower())))
                {
                    continue;
                }

                if (setting.DenyMethods.Exists(x => x.ToLower() == method))
                {
                    continue;
                }

                if (setting.AllowMethods.Any() && !setting.AllowMethods.Exists(x => x.ToLower() == method))
                {
                    continue;
                }

                if (setting.DenyStatusCodes.Exists(x => x == statusCode))
                {
                    continue;
                }

                if (setting.AllowStatusCodes.Any() && !setting.AllowStatusCodes.Exists(x => x == statusCode))
                {
                    continue;
                }

                if (setting.DenyPaths.Exists(x => path.Contains(x.ToLower())))
                {
                    continue;
                }

                if (setting.AllowPaths.Any() && !setting.AllowPaths.Exists(x => path.Contains(x.ToLower())))
                {
                    continue;
                }
                return true;
            }

            return false;
        }
    }
}
