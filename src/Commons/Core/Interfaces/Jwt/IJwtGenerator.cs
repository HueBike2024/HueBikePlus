using Core.Models.Settings;
using System.Security.Claims;

namespace Core.Interfaces.Jwt
{
    public interface IJwtGenerator
    {
        /// <summary>
        /// Generate the Jwt token with settings and claims information
        /// </summary>
        /// <param name="settings">JWT token settings</param>
        /// <param name="claims">Claims information</param>
        /// <returns>JWT token string</returns>
        string Generate(JwtTokenSetting settings, ClaimsIdentity claims);
    }
}
