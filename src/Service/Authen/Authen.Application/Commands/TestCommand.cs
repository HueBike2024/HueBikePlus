using Core.Interfaces.Database;
using Core.SeedWork.Repository;
using Infrastructure.AggregatesModel.Authen.AccountAggregate;
using MediatR;

namespace Authen.Application.Commands
{
    public class TestCommand : IRequest<bool>
    {
        public int MyProperty { get; set; }
    }

    public class TestCommandHandler : IRequestHandler<TestCommand, bool>
    {
        private readonly IRepository<User> _userRep;
        private readonly IUnitOfWork _unitOfWork;
        public TestCommandHandler(IRepository<User> userRep, IUnitOfWork unitOfWork)
        {
            _userRep = userRep;
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(TestCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRep.FindOneAsync(e => e.IsActive);

            if (user != null) return true;

            return false;
        }
    }
}
