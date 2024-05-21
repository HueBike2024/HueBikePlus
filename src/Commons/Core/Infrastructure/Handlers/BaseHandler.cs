using AutoMapper;
using Core.Extensions;
using Core.Implements.Http;
using Core.Interfaces.Logging;
using MediatR;

namespace Core.Infrastructure.Handlers
{
    public abstract class BaseHandler
    {
        protected readonly IMapper Mapper;
        protected readonly IAppLogger Logger;
        protected readonly IMediator Mediator;

        protected readonly long UserId;
        protected readonly string UserName;
        //protected readonly string UserFullName;
        protected readonly string Token;
        protected readonly bool IsSuperAdmin;
        protected BaseHandler()
        {
            Mapper = HttpAppService.GetRequestService<IMapper>();
            Logger = HttpAppService.GetRequestService<IAppLogger>();
            Mediator = HttpAppService.GetRequestService<IMediator>();

            UserId = long.Parse(TokenExtensions.GetUserId());
            UserName = TokenExtensions.GetUserName();
            //UserFullName = TokenExtensions.GetName();
            Token = TokenExtensions.GetToken();
            IsSuperAdmin = TokenExtensions.IsSuperAdmin();
        }
    }
}
