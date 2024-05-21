using Core.Models.Base;

namespace Core.Interfaces.Jwt
{
    public interface IJwtProvider
    {
        string GenerateJwtToken(BaseUser user, int? tokenLifeTime = null);

        bool ValidateAudience(string issuer
            , string secretKey
            , string audienceName
            , ref string errorMessage);

        string GenerateRefreshToken();
        string GetPropertyValue(string claimType);
    }
}
