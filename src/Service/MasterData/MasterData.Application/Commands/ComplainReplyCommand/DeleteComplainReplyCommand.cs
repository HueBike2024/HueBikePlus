using Core.Exceptions;
using Core.Interfaces.Database;
using Core.Properties;
using Core.SeedWork.Repository;
using Infrastructure.AggregatesModel.Authen.AccountAggregate;
using Infrastructure.AggregatesModel.Authen;
using Infrastructure.AggregatesModel.MasterData.UserAggregate.ComplainAggregate;
using MasterData.Application.Commands.ComplainCommand;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Services;

namespace MasterData.Application.Commands.ComplainReplyCommand
{
    public class DeleteComplainReplyCommand : IRequest<bool>
    {
        public long Id { get; set; }    
    }

    public class DeleteComplainReplyCommandHandler : IRequestHandler<DeleteComplainReplyCommand, bool>
    {
        private readonly IRepository<Complain> _compRep;
        private readonly IFileService _fileService;
        private readonly IRepository<User> _userRep;
        private readonly IRepository<ComplainReply> _replyRep;
        private readonly IRepository<AuthenMedia> _media;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteComplainReplyCommandHandler(IRepository<Complain> compRep, IRepository<ComplainReply> replyRep, IUnitOfWork unitOfWork, IRepository<User> userRep, IFileService fileService)
        {
            _compRep = compRep;
            _userRep = userRep;
            _replyRep = replyRep;
            _unitOfWork = unitOfWork;
            _fileService = fileService;
        }

        public async Task<bool> Handle(DeleteComplainReplyCommand request, CancellationToken cancellationToken)
        {
            var reply = await _replyRep.FindOneAsync(e => e.Id == request.Id);

            if (reply == null)
            {
                throw new BaseException(ErrorsMessage.MSG_NOT_EXIST, "Câu trả lời");
            }

            _replyRep.Remove(reply);

            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}
