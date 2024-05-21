using Core.Interfaces.Database;
using Core.SeedWork.Repository;
using Infrastructure.AggregatesModel.Authen.AccountAggregate;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Authen.Application.Commands.AccountCommand
{
    public class RevokeTokenCommand : IRequest<bool>
    {
        public long Id { get; set; }
    }

    public class RevokeTokenCommandHandler : IRequestHandler<RevokeTokenCommand, bool>
    {
        private readonly IRepository<UserToken> _userTokenRep;
        private readonly IUnitOfWork _unitOfWork;

        public RevokeTokenCommandHandler(IRepository<UserToken> userTokenRep,IUnitOfWork unitOfWork)
        {
            _userTokenRep = userTokenRep;
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(RevokeTokenCommand request, CancellationToken cancellationToken)
        {

            List<UserToken> accessTokens = new();

            var accessToken = await _userTokenRep.GetQuery(e => e.UserId == request.Id)
                .Include(x => x.RefreshToken)
                .FirstOrDefaultAsync(cancellationToken);

            if (accessToken != null)
                accessTokens.Add(accessToken);

            if (accessTokens.Count == 0)
                return true;

            accessTokens.ForEach(e =>
            {
                e.IsActive = false;
                e.RefreshToken.IsActive = false;
            });

            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}
