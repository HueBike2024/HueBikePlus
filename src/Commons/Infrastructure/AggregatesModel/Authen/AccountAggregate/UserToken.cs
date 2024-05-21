using Core.Models.Interface;

namespace Infrastructure.AggregatesModel.Authen.AccountAggregate
{
    public class UserToken : IEntity
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public long RefreshTokenId { get; set; }
        public string Token { get; set; }
        public DateTime ExpiredTime { get; set; }
        public bool IsActive { get; set; }
        public string IP { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public virtual User User { get; set; }
        public virtual UserRefreshToken RefreshToken { get; set; }

        public UserToken()
        {

        }

        public static UserToken CreateUserToken(long userId, string token, DateTime expiredTime, string ip, bool isActive = true)
        {
            return new UserToken
            {
                UserId = userId,
                Token = token,
                ExpiredTime = expiredTime,
                IP = ip,
                IsActive = isActive
            };
        }
    }
}