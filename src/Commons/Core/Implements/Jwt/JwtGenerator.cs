using Core.Interfaces.Jwt;
using Core.Models.Settings;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Core.Implements.Jwt
{
    public class JwtGenerator : IJwtGenerator
    {
        public string Generate(JwtTokenSetting settings, ClaimsIdentity claims)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(settings.SecretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

            var expiredTime = DateTime.UtcNow.AddMinutes(settings.TokenLifeTimeForWeb);

            var jwtTokenHandler = new JwtSecurityTokenHandler();

            var tokenDescription = new SecurityTokenDescriptor
            {
                Subject = claims,
                Expires = expiredTime,
                SigningCredentials = creds,
                Issuer = settings.Issuer,
                Audience = settings.Audience

            };
            var token = jwtTokenHandler.CreateToken(tokenDescription);

            return jwtTokenHandler.WriteToken(token);
        }
    }
}
