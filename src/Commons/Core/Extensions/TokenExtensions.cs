using Microsoft.AspNetCore.Http;
using NodaTime;
using NodaTime.Extensions;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using static Core.Common.AppConstants;

namespace Core.Extensions
{
    public static class TokenExtensions
    {
        private static IHttpContextAccessor _httpContextAccessor;
        public static void Configure(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// Get User Id
        /// </summary>
        /// <returns></returns>
        public static string GetUserId()
        {
            if (_httpContextAccessor == null) return null;
            var userClaim = _httpContextAccessor.HttpContext?.GetClaims();
            var nguoiDungId = userClaim?.FirstOrDefault(x => x.Type.Contains(ClaimType.UserId))?.Value ?? "0";

            return nguoiDungId;
        }

        /// <summary>
        /// Get User name
        /// </summary>
        /// <returns></returns>
        public static string GetUserName()
        {
            if (_httpContextAccessor == null) return null;
            var userClaim = _httpContextAccessor.HttpContext?.GetClaims();
            var userName = userClaim?.FirstOrDefault(x => x.Type.Contains(ClaimType.UserName))?.Value;

            return userName;
        }

        private static List<Claim> GetClaims(this HttpContext httpContext)
        {
            var token = httpContext.GetToken();
            if (string.IsNullOrEmpty(token))
            {
                return new List<Claim>();
            }
            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            try
            {
                return jwtSecurityTokenHandler.ReadJwtToken(token).Claims.ToList();
            }
            catch
            {
                return new List<Claim>();
            }
        }

        public static string GetIP()
        {
            string authHeader = _httpContextAccessor.HttpContext.Request.Headers[Auth.IP];
            if (authHeader == null) return null;
            var bearerToken = authHeader/*.Substring(Auth.BearerHeader.Length)*/
                .Trim();
            return bearerToken;
        }

        private static string GetToken(this HttpContext httpContext)
        {
            if (httpContext == null)
            {
                return string.Empty;
            }

            string authHeader = httpContext.Request.Headers[Auth.Authorization];
            if (authHeader == null) return null;
            var bearerToken = authHeader.Substring(Auth.BearerHeader.Length)
                .Trim();
            return bearerToken;
        }

        public static string GetToken()
        {
            string authHeader = _httpContextAccessor.HttpContext.Request.Headers[Auth.Authorization];
            if (authHeader == null) return null;
            var bearerToken = authHeader.Substring(Auth.BearerHeader.Length)
                .Trim();

            return bearerToken;
        }

        /// <summary>
        /// Get IsSuperAdmin
        /// </summary>
        /// <returns></returns>
        public static bool IsSuperAdmin()
        {
            if (_httpContextAccessor == null) return false;
            var userClaim = _httpContextAccessor.HttpContext?.GetClaims();
            var isSuperAdmin = userClaim?.FirstOrDefault(x => x.Type.Contains(ClaimType.IsSuperAdmin))?.Value ?? "False";
            return bool.Parse(isSuperAdmin);
        }

        /// <summary>
        /// Get Timezone
        /// </summary>
        /// <returns></returns>
        public static string GetTimeZone()
        {
            if (_httpContextAccessor == null) return Core.Common.Constants.DEFAULT_TIME_ZONE;
            var userClaim = _httpContextAccessor.HttpContext?.GetClaims();
            string timeZoneRequest = _httpContextAccessor.HttpContext?.Request?.Headers?.FirstOrDefault(e => e.Key == ClaimType.TimeZone).Value;
            if(userClaim?.Count > 0)
            {
                string timeZone = userClaim?.FirstOrDefault(x => x.Type.Contains(ClaimType.TimeZone))?.Value;

                // Ưu tiên TimeZone của user trong Token
                return string.IsNullOrEmpty(timeZone) 
                        ? (timeZoneRequest ?? Core.Common.Constants.DEFAULT_TIME_ZONE)
                        : timeZone;
            }

            return timeZoneRequest ?? Core.Common.Constants.DEFAULT_TIME_ZONE;
        }

        public static DateTime TimeZoneConversion(DateTime timeUTC)
        {
            // the time zone to convert to
            var timeZone = DateTimeZoneProviders.Tzdb[GetTimeZone()];
            // the date as UTC - this could be from a data store
            // convert to instant from UTC - see http://stackoverflow.com/questions/20807799/using-nodatime-how-to-convert-an-instant-to-the-corresponding-systems-zoneddat
            var instant = Instant.FromDateTimeUtc(DateTime.SpecifyKind(timeUTC, DateTimeKind.Utc));
            var result = instant.InZone(timeZone).ToDateTimeUnspecified();

            return result;
        }
    }
}
