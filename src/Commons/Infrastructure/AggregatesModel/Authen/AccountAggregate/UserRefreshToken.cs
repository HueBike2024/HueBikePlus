using Core.Models.Interface;

namespace Infrastructure.AggregatesModel.Authen.AccountAggregate
{
    public class UserRefreshToken : IEntity
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public string RefreshToken { get; set; }
        public DateTime RefreshExpiredTime { get; set; }
        public bool IsActive { get; set; }

        public virtual User User { get; set; }
        public virtual ICollection<UserToken> UserTokens { get; set; }

        public UserRefreshToken()
        {

        }

        public static UserRefreshToken CreateUserRefreshToken(long userId, string refreshToken, DateTime refreshExpiredTime, ICollection<UserToken> userTokens, bool isActive = true)
        {
            return new UserRefreshToken
            {
                UserId = userId,
                RefreshToken = refreshToken,
                RefreshExpiredTime = refreshExpiredTime,
                UserTokens = userTokens,
                IsActive = isActive
            };
        }
    }
}