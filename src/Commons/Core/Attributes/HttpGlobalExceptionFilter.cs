using EVN.Domain.Utility;
using Core.Common;
using Core.Exceptions;
using Core.Models;
using Core.Properties;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Context;
using System.Linq;
using System.Net;

namespace Core.Attributes
{
    public class HttpGlobalExceptionFilter : IExceptionFilter
    {
        private readonly IWebHostEnvironment _env;
        private readonly ILogger<HttpGlobalExceptionFilter> _logger;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="env"></param>
        /// <param name="logger"></param>
        public HttpGlobalExceptionFilter(IWebHostEnvironment env, ILogger<HttpGlobalExceptionFilter> logger)
        {
            _env = env;
            _logger = logger;
        }

        /// <summary>
        /// System exception
        /// </summary>
        /// <param name="context"></param>
        public void OnException(ExceptionContext context)
        {
            var exception = context.Exception;
            var developerMessage = exception.Message + "\r\n" + exception.StackTrace;
            while (exception.InnerException != null)
            {
                developerMessage += "\r\n--------------------------------------------------\r\n";
                exception = exception.InnerException;
                developerMessage += (exception.Message + "\r\n" + exception.StackTrace);
            }

            _logger.LogError(new EventId(context.Exception.HResult),
                context.Exception,
                developerMessage);

            if (context.ModelState.ErrorCount > 0)
            {
                var errors = context.ModelState.Where(v => v.Value.Errors.Count > 0)
                    .ToDictionary(
                        kvp => $"{char.ToLower(kvp.Key[0])}{kvp.Key.Substring(1)}",
                        kvp => kvp.Value.Errors.FirstOrDefault()?.ErrorMessage
                    );

                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.UnprocessableEntity;
                context.Result = new UnprocessableEntityObjectResult(new JsonResponse
                {
                    StatusCode = StatusCodes.Status422UnprocessableEntity,
                    //Message = BaseResources.MSG_INVALID_DATA,
                    Data = errors
                });
                context.ExceptionHandled = true;
                return;
            }

            var json = new JsonResponse
            {
                Message = context.Exception.Message
            };

            if (_env.EnvironmentName != "Production")
                json.DeveloperMessage = developerMessage;

            var userName = context.HttpContext.User.Identity.IsAuthenticated
                ? context.HttpContext.User.Identity.Name : "Guest"; //Gets user Name from user Identity 
            var ipAddress = HttpRequestUtility.GetIpAddress(context.HttpContext);
            LogContext.PushProperty("UserName", userName);
            //LogContext.PushProperty("IP", ipAddress);
            //LogContext.PushProperty("LogEvent", null);
            // 400 Bad Request
            if (context.Exception.GetType() == typeof(BaseException))
            {
                var errorCode = (int?)exception.Data[BaseException.ErrorCode];
                if (errorCode != null)
                {
                    json.StatusCode = errorCode.Value;
                    context.HttpContext.Response.StatusCode = errorCode.Value;
                }
                else
                {
                    json.StatusCode = StatusCodes.Status400BadRequest;
                    context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                }
                context.Result = new BadRequestObjectResult(json);
                //Log.Logger.Warning(developerMessage);
            }
            // 404 Not Found
            else if (context.Exception.GetType() == typeof(NotFoundException))
            {
                json.StatusCode = StatusCodes.Status404NotFound;
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.NotFound;
                context.Result = new NotFoundObjectResult(json);
                //Log.Logger.Error(developerMessage);
            }
            // 500 Internal Server Error
            else
            {
                json.Message = ErrorsMessage.MSG_SYSTEM_ERROR;
                json.StatusCode = StatusCodes.Status500InternalServerError;
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Result = new InternalServerErrorObjectResult(json);
                //Log.Logger.Error(developerMessage);
                //LogHelper.ErrorSystemLogger.Error(context.Exception, ErrorsMessage.MSG_SYSTEM_ERROR);
            }
            context.ExceptionHandled = true;
        }
    }
}
