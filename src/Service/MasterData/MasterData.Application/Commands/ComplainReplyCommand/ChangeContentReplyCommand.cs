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
    public class ChangeContentReplyCommand : IRequest<bool>
    {
        public long ReplyId {  get; set; }
        public string Content { get; set; }
    }

    public class ChangeContentReplyCommandHandler : IRequestHandler<ChangeContentReplyCommand, bool>
    {
        private readonly IRepository<Complain> _compRep;
        private readonly IFileService _fileService;
        private readonly IRepository<User> _userRep;
        private readonly IRepository<ComplainReply> _replyRep;
        private readonly IRepository<AuthenMedia> _media;
        private readonly IUnitOfWork _unitOfWork;

        public ChangeContentReplyCommandHandler(IRepository<ComplainReply> replyRep, IUnitOfWork unitOfWork)
        {
            _replyRep = replyRep;
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(ChangeContentReplyCommand request, CancellationToken cancellationToken)
        {
            var reply = await _replyRep.FindOneAsync(e => e.Id == request.ReplyId);

            if (reply == null)
            {
                throw new BaseException(ErrorsMessage.MSG_NOT_EXIST, "Câu trả lời");
            }

            reply.Content = request.Content;
            _replyRep.Update(reply);

            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}
