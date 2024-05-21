using Core.Exceptions;
using Core.Infrastructure.Handlers;
using Core.Interfaces.Database;
using Core.Properties;
using Core.SeedWork.Repository;
using Infrastructure.AggregatesModel.Authen;
using Infrastructure.AggregatesModel.Authen.AccountAggregate;
using Infrastructure.AggregatesModel.MasterData.ImageAggregate;
using Infrastructure.AggregatesModel.MasterData.NotificationAggregate;
using Infrastructure.AggregatesModel.MasterData.UserAggregate.ComplainAggregate;
using Infrastructure.Services;
using MasterData.Application.Commands.UnitCommand;
using MasterData.Application.DTOs.Complain;
using MasterData.Application.Services.CloudinaryService;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterData.Application.Commands.ComplainCommand
{
    public class CreateComplainCommand : IRequest<ComplainResponse>
    {
        public IFormFile ImageFile { get; set; }
        public string Content { get; set; }
    }

    public class CreateComplainCommandHandler : BaseHandler, IRequestHandler<CreateComplainCommand, ComplainResponse>
    {
        public readonly IRepository<Complain> _compRep;
        public readonly IRepository<User> _userRep;
        public readonly IUnitOfWork _unitOfWork;
        private readonly ICloudPhotoService _cloudService;
        private readonly IRepository<Avatar> _avatar;

        public CreateComplainCommandHandler(IRepository<Avatar> avatar, ICloudPhotoService cloudService, IRepository<Complain> compRep, IUnitOfWork unitOfWork, IRepository<User> userRep)
        {
            _compRep = compRep;
            _userRep = userRep;
            _unitOfWork = unitOfWork;
            _cloudService = cloudService;
            _avatar = avatar;
        }
        public async Task<ComplainResponse> Handle(CreateComplainCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRep.FindOneAsync(e => e.Id == UserId);

            if (user == null)
            {
                throw new BaseException(ErrorsMessage.MSG_NOT_EXIST, "Người dùng");
            }

            if (request.Content.Length > 500)
            {
                throw new BaseException(ErrorsMessage.MSG_MAX_LENGTH, "Nội dung không quá 500 kí tự");
            }

            //Tạo mới complain
            var complain = new Complain(request.Content, "", UserId);

            if (request.ImageFile != null && request.ImageFile.Length > 0)
            {

                var uploadResult = await _cloudService.UploadPhotoAsync(request.ImageFile, "notification");

                if (uploadResult != null)
                {
                    var avatar = new Avatar(uploadResult.PublicId, uploadResult.Url);
                    _avatar.Add(avatar);

                    await _unitOfWork.SaveChangesAsync();

                    complain.Image = avatar.ImageUrl;
                }
                else
                {
                    throw new BaseException("Không thể tải lên hình ảnh!");
                }
            }

            _compRep.Add(complain);

            await _unitOfWork.SaveChangesAsync();

            var complainResponse = new ComplainResponse
            {
                Id = complain.Id,
                SenderId = complain.SenderId,
                SenderUsername = user.UserName,
                Image = complain.Image,
                Content = complain.Content,
                CreatedDate = complain.CreatedDate,
            };

            return complainResponse;
        }
    }
}
