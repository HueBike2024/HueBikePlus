using Core.Exceptions;
using Core.Interfaces.Database;
using Core.Properties;
using Core.SeedWork.Repository;
using Infrastructure.AggregatesModel.MasterData.BikeManagementAggregate.BikeAggregate;
using Infrastructure.AggregatesModel.MasterData.PostAggregate;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterData.Application.Commands.PostCommand
{
    public class DeletePostCommand : IRequest<bool>
    {
        public long Id { get; set; }
    }

    public class DeletePostCommandHandler : IRequestHandler<DeletePostCommand, bool>
    {

        private readonly IRepository<Post> _postRep;
        private readonly IUnitOfWork _unitOfWork;

        public DeletePostCommandHandler(IRepository<Post> postRep, IUnitOfWork unitOfWork)
        {
            _postRep = postRep;
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(DeletePostCommand request, CancellationToken cancellationToken)
        {
            var post = await _postRep.FindOneAsync(e => e.Id == request.Id);

            if (post == null)
            {
                throw new BaseException(ErrorsMessage.MSG_NOT_EXIST, "Post");
            }



            Post.DeletePost(ref post);


            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}
