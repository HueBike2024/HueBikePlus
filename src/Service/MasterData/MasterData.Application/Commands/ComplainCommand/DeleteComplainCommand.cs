using Core.Exceptions;
using Core.Interfaces.Database;
using Core.Properties;
using Core.SeedWork.Repository;
using Infrastructure.AggregatesModel.Authen;
using Infrastructure.AggregatesModel.Authen.AccountAggregate;
using Infrastructure.AggregatesModel.MasterData.UserAggregate.ComplainAggregate;
using Infrastructure.Services;
using MasterData.Application.Commands.UnitCommand;
using MasterData.Application.DTOs.Complain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterData.Application.Commands.ComplainCommand
{
    public class DeleteComplainCommand : IRequest<bool>
    {
        public long ComplainId { get; set; }
        public long UserId { get; set; }
    }

    public class DeleteComplainCommandHandler : IRequestHandler<DeleteComplainCommand, bool>
    {
        private readonly IRepository<Complain> _compRep;
        private readonly IFileService _fileService;
        private readonly IRepository<User> _userRep;
        private readonly IRepository<ComplainReply> _replyRep;
        private readonly IRepository<AuthenMedia> _media;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteComplainCommandHandler(IRepository<Complain> compRep, IRepository<ComplainReply> replyRep, IUnitOfWork unitOfWork, IRepository<User> userRep, IFileService fileService)
        {
            _compRep = compRep;
            _userRep = userRep;
            _replyRep = replyRep;
            _unitOfWork = unitOfWork;
            _fileService = fileService;
        }

        public async Task<bool> Handle(DeleteComplainCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRep.FindOneAsync(e => e.Id == request.UserId);

            if (user == null)
            {
                throw new BaseException(ErrorsMessage.MSG_NOT_EXIST, "Người dùng");
            }

            var complain = await _compRep.FindOneAsync(e => e.Id == request.ComplainId);

            if (complain == null)
            {
                throw new BaseException(ErrorsMessage.MSG_NOT_EXIST, "Khiếu nại");
            }

            var replies = await _replyRep.GetQuery().Where(e => e.ComplainId == request.ComplainId).ToListAsync();

            foreach (var rep in replies)
            {
                _replyRep.Remove(rep);
            }

            _fileService.DeleteFile(complain.Image);
            _compRep.Remove(complain);

            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}
