using Core.Implements.Http;
using MediatR;

namespace Core.Infrastructure.Services
{
    public abstract class BaseService
    {
        protected readonly IMediator Mediator;

        protected BaseService()
        {
            Mediator = HttpAppService.GetRequestService<IMediator>();
        }
    }
}
