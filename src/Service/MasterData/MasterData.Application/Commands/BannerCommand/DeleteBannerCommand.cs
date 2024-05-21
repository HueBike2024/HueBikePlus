using Core.Exceptions;
using Core.Interfaces.Database;
using Core.Properties;
using Core.SeedWork.Repository;
using Infrastructure.AggregatesModel.MasterData.BannerAggregate;
using MediatR;

namespace MasterData.Application.Commands.BannerCommand
{
    public class DeleteBannerCommand : IRequest<bool>
    {
        public long Id { get; set; }
    }

    public class DeleteBannerCommandHandler : IRequestHandler<DeleteBannerCommand, bool>
    {

        private readonly IRepository<Banner> _banRep;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteBannerCommandHandler(IRepository<Banner> banRep, IUnitOfWork unitOfWork)
        {
            _banRep = banRep;
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(DeleteBannerCommand request, CancellationToken cancellationToken)
        {
            var banner = await _banRep.FindOneAsync(e => e.Id == request.Id);

            if (banner == null)
            {
                throw new BaseException(ErrorsMessage.MSG_NOT_EXIST, "Banner");
            }



            Banner.DeleteBanner(ref banner);


            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}
