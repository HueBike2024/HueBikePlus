using MasterData.Application.DTOs.Account;
using Core.Helpers.Cryptography;
using Core.Interfaces.Database;
using Core.Interfaces.Jwt;
using Core.Properties;
using Core.SeedWork.Repository;
using Infrastructure.AggregatesModel.Authen.AccountAggregate;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace MasterData.Application.Commands.AccountCommand
{
    public class SignInCommand: IRequest<SignInResponse>
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string UrlRedirect { get; set; }
        public string IP { get; set; }
    }
    
    public class SignInCommandHandler: IRequestHandler<SignInCommand, SignInResponse>
    {
        private readonly IRepository<User> _userRep;
        private readonly IRepository<UserRefreshToken> _userRefreshTokenRep;
        private readonly IJwtProvider _jwtProvider;
        private readonly IUnitOfWork _unitOfWork;

        public SignInCommandHandler(IRepository<User> userRep, IRepository<UserRefreshToken> userRefreshTokenRep, IJwtProvider jwtProvider, IUnitOfWork unitOfWork)
        {
            _userRep = userRep;
            _userRefreshTokenRep = userRefreshTokenRep;
            _jwtProvider = jwtProvider;
            _unitOfWork = unitOfWork;
        }

        public async Task<SignInResponse> Handle(SignInCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRep.GetQuery(e => e.UserName == request.UserName)
                .Include(e => e.Avatar)
                .Include(e => e.UserTokens.Where(e => e.IsActive && e.ExpiredTime < DateTime.UtcNow))
                .Include(e => e.UserRefreshTokens.Where(e => e.IsActive && e.RefreshExpiredTime < DateTime.UtcNow))
                .FirstOrDefaultAsync(cancellationToken);

            if (user == null)
                return new SignInResponse(ErrorsMessage.MSG_ERROR_LOGIN, StatusCodes.Status422UnprocessableEntity);
            if(user.IsLock == true)
            {
                return new SignInResponse(ErrorsMessage.MSG_VALID_BLOCK_AUTH, StatusCodes.Status422UnprocessableEntity);
            }

            if (!user.IsSuperAdmin)
            {
                // TODO: Xác nhận mail

                //if (!user.IsActive)
                //    throw new BaseException(Resource.MSG_VALID_BLOCK_AUTH);

                if (!user.IsActive)
                    return new SignInResponse(ErrorsMessage.MSG_VALID_BLOCK_AUTH, StatusCodes.Status422UnprocessableEntity);
            }

            if (SHACryptographyHelper.ComparePasswords(request.Password, user.Password) == false)
                return new SignInResponse(ErrorsMessage.MSG_ERROR_PASS_WORD, StatusCodes.Status422UnprocessableEntity);

            // Delete token old not active
            var listTask = new List<Task>();

            var userTokens = user.UserTokens;
            listTask.Add(Task.Run(() => UnActiveUserToken(ref userTokens), cancellationToken));

            var refreshTokens = user.UserRefreshTokens;
            listTask.Add(Task.Run(() => UnActiveRefreshToken(ref refreshTokens), cancellationToken));

            // Generate token new
            var expiredToken = DateTime.UtcNow.AddMinutes(Constants.Constants.TokenLifeTime);
            var accessToken = _jwtProvider.GenerateJwtToken(user, Constants.Constants.TokenLifeTime);
            var refreshToken = _jwtProvider.GenerateRefreshToken();

            // Save user token
            //var tokenSettings = _configuration.GetSection(CONFIG_KEYS.APP_SETTING).Get<AppSettings>()!.Jwt;
            //var expiredToken = DateTime.UtcNow.AddMinutes(tokenSettings.TokenLifeTimeForWeb);

            var userToken = UserToken.CreateUserToken(user.Id, accessToken, expiredToken, request.IP);

            //var expiredRefreshToken = DateTime.UtcNow.AddMinutes(tokenSettings.RefreshTokenLifeTimeWeb);
            var expiredRefreshToken = DateTime.UtcNow.AddMinutes(Constants.Constants.RefreshTokenLifeTime);
            var refreshTokenObj = UserRefreshToken.CreateUserRefreshToken(user.Id, refreshToken, expiredRefreshToken, new List<UserToken> { userToken });
            _userRefreshTokenRep.Add(refreshTokenObj);

            await Task.WhenAll(listTask);

            await _unitOfWork.SaveChangesAsync();

            List<string> roles = new List<string>();
            if (user.IsSuperAdmin)
                roles.Add("Admin");
            else
                roles.Add("Customer");

            return new SignInResponse
            {
                AccessToken = accessToken,
                ExpiredToken = expiredToken,
                RefreshToken = refreshToken,
                ExpiredRefreshToken = expiredRefreshToken,
                UrlRedirect = request.UrlRedirect,
                IsSuperAdmin = user.IsSuperAdmin,
                StatusCode = StatusCodes.Status200OK,
                Roles = roles,
                InfoUser = new InfoUser
                {
                    Id = user.Id,
                    FullName = user.FullName,
                    UserName = user.UserName,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    Address = user.Address,
                    DateOfBirth = user.DateOfBirth,
                    TimeZone = user.TimeZone,
                },
            };
        }

        private static void UnActiveUserToken(ref ICollection<UserToken> tokens)
        {
            if (tokens.Count < 1) return;
            foreach (var token in tokens)
            {
                token.IsActive = false;
            }
        }

        private static void UnActiveRefreshToken(ref ICollection<UserRefreshToken> tokens)
        {
            if (tokens.Count < 1) return;
            foreach (var token in tokens)
            {
                token.IsActive = false;
            }
        }
    }
}
