using Core.Implements.Http;
using Core.Interfaces.Jwt;
using Core.Models.Base;
using Core.Models.Settings;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;
using System.Security.Cryptography;
using static Core.Common.Constants;

namespace Core.Implements.Jwt
{
    public class JwtProvider : IJwtProvider
    {
        private readonly IConfiguration _configuration;
        private readonly IJwtGenerator _jwtGenerator;

        public JwtProvider(IConfiguration configuration
            , IJwtGenerator jwtGenerator
            )
        {
            _configuration = configuration;
            _jwtGenerator = jwtGenerator;
        }

        public string GenerateJwtToken(BaseUser user, int? tokenLifeTime = null)
        {
            var tokenSettings = _configuration.GetSection(CONFIG_KEYS.APP_SETTING).Get<AppSettings>()!.Jwt;
            var claimArrays = new[]
            {
                new Claim(Common.AppConstants.ClaimType.UserId, user.Id.ToString()),
                new Claim(Common.AppConstants.ClaimType.UserName, user.UserName),                           
                new Claim(Common.AppConstants.ClaimType.IsSuperAdmin, user.IsSuperAdmin.ToString()),
                new Claim(Common.AppConstants.ClaimType.TimeZone, user.TimeZone ?? string.Empty),
            };

            var claims = new ClaimsIdentity(claimArrays);

            if (tokenLifeTime.HasValue)
            {
                tokenSettings.TokenLifeTimeForWeb = tokenLifeTime.Value;
                tokenSettings.TokenLifeTimeForMobile = tokenLifeTime.Value;
            }
            return _jwtGenerator.Generate(tokenSettings, claims);
        }

        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        public string GetPropertyValue(string claimType)
        {
            var claims = HttpAppContext.Current.User.Identity as ClaimsIdentity;
            var response = claims.Claims.FirstOrDefault(c => c.Type == claimType)?.Value;

            return response;
        }

        public bool ValidateAudience(string issuer
            , string secretKey
            , string audienceName
            , ref string errorMessage)
        {
            var tokenSettings = _configuration.GetSection(CONFIG_KEYS.JWT_TOKEN).Get<JwtTokenSetting>();

            if (tokenSettings.Issuer != issuer)
            {
                errorMessage = string.Format("You do not have permission", nameof(tokenSettings.Issuer));
                return false;
            }

            if (tokenSettings.SecretKey != secretKey)
            {
                errorMessage = string.Format("You do not have permission", nameof(tokenSettings.SecretKey));
                return false;
            }

            return true;
        }
    }
}
