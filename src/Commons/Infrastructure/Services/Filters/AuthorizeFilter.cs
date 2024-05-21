using Core.Extensions;
using Infrastructure.EF;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;

namespace Infrastructure.Services.Filters
{
    public class AuthorizeFilter : IAuthorizationFilter
    {
        private readonly List<string> _privilegeValue;
        private readonly BaseDbContext _dbContext;

        public AuthorizeFilter(string privilegeValue, BaseDbContext dbContext)
        {
            _privilegeValue = privilegeValue.Split(",").Select(c => c.Trim()).ToList();
            _dbContext = dbContext;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (!context.ModelState.IsValid)
            {
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                //context.Result = new RedirectResult($"tai-khoan/error/{(int)HttpStatusCode.Unauthorized}");
                context.Result = new StatusCodeResult((int)HttpStatusCode.Unauthorized);
                return;
            }

            // Check if not need privilege  
            if (_privilegeValue.Contains("*"))
            {
                return;
            }

            #region Kiểm tra Token
            var principal = context.HttpContext.User;
            if (principal == null)
            {
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                //context.Result = new RedirectResult($"tai-khoan/error/{(int)HttpStatusCode.Unauthorized}");
                context.Result = new StatusCodeResult((int)HttpStatusCode.Unauthorized);
                return;
            }

            var userName = string.Empty;
            var token = TokenExtensions.GetToken();
            if (token == null)
            {
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                context.Result = new RedirectResult($"account/error/{(int)HttpStatusCode.Unauthorized}");
                return;
            }

            if (principal.Identity != null && !principal.Identity.IsAuthenticated && string.IsNullOrEmpty(token))
            {
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                //context.Result = new RedirectResult($"tai-khoan/error/{(int)HttpStatusCode.Unauthorized}");
                context.Result = new StatusCodeResult((int)HttpStatusCode.Unauthorized);
                return;
            }
            else
            {
                if (!string.IsNullOrEmpty(token))
                {
                    userName = TokenExtensions.GetUserName();
                }
            }

            var userIP = TokenExtensions.GetIP();
            var user = _dbContext.Users.FirstOrDefault(e => e.UserName == userName && !e.IsDeleted);

            if (user == null)
            {
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                //context.Result = new RedirectResult($"tai-khoan/error/{(int)HttpStatusCode.Unauthorized}");
                context.Result = new StatusCodeResult((int)HttpStatusCode.Unauthorized);
                return;
            }

            // Kiểm tra super admin
            if (user.IsSuperAdmin)
                return;

            // Kiểm tra Hoạt động
            if (!user.IsActive)
            {
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                context.Result = new StatusCodeResult((int)HttpStatusCode.Unauthorized);
                return;
            }

            long userId = long.Parse(TokenExtensions.GetUserId());
            var login = _dbContext.UserTokens.FirstOrDefault(e => e.UserId == userId && e.IsActive && e.ExpiredTime >= DateTime.UtcNow && e.Token == token);

            if (login == null)
            {
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                context.Result = new StatusCodeResult((int)HttpStatusCode.Unauthorized);
                return;
            }

            #endregion

            //#region Kiểm tra quyền người dùng - chức năng
            //var actionName = ((Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor)context.ActionDescriptor).ActionName;
            //var controllerName = ((Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor)context.ActionDescriptor).ControllerName;

            //var userPermission = _dbContext.UserFunctions.FirstOrDefault(e => e.UserId == user.Id
            //     && e.Function.ControllerName.ToLower() == controllerName.ToLower());

            //if (userPermission == null)
            //{
            //    context.HttpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            //    context.Result = new StatusCodeResult((int)HttpStatusCode.Forbidden);
            //    return;
            //}

            //var previleges = userPermission.ListPermission;
            //bool hasPrevilege = false;

            //foreach (var previlege in _privilegeValue)
            //{
            //    if (previleges.Contains(previlege))
            //    {
            //        hasPrevilege = true;
            //        break;
            //    }
            //}
            //if (!hasPrevilege)
            //{
            //    context.HttpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            //    context.Result = new StatusCodeResult((int)HttpStatusCode.Forbidden);
            //    return;
            //}

            //#endregion
        }
    }
}