using Core.Exceptions;
using Core.Interfaces.Database;
using Core.Properties;
using Core.SeedWork.Repository;
using GoogleApi.Entities.Maps.Common;
using Infrastructure.AggregatesModel.Authen;
using Infrastructure.AggregatesModel.Authen.AccountAggregate;
using Infrastructure.AggregatesModel.MasterData.ImageAggregate;
using Infrastructure.AggregatesModel.MasterData.NotificationAggregate;
using Infrastructure.Services;
using MasterData.Application.Commands.UnitCommand;
using MasterData.Application.Services.CloudinaryService;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterData.Application.Commands.NotificationCommand
{
    public class CreateNotificationCommand : IRequest<long>
    {
        public long UserId { get; set; }
        public string Title { get; set; }
        public IFormFile ImageFile { get; set; }
        public string Content { get; set; }
    }

    public class CreateNotificationCommandHandler : IRequestHandler<CreateNotificationCommand, long>
    {
        public readonly IRepository<User> _userRep;
        public readonly IRepository<Notification> _notiRep;
        public readonly IUnitOfWork _unitOfWork;
        private readonly ICloudPhotoService _cloudService;
        private readonly IRepository<Avatar> _avatar;

        public CreateNotificationCommandHandler(IRepository<Avatar> avatar, ICloudPhotoService cloudService, IRepository<User> userRep, IUnitOfWork unitOfWork, IRepository<Notification> notiRep)
        {
            _userRep = userRep;
            _notiRep = notiRep;
            _unitOfWork = unitOfWork;
            _cloudService = cloudService;
            _avatar = avatar;
        }
        public async Task<long> Handle(CreateNotificationCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRep.FindOneAsync(e => e.Id == request.UserId);

            if (user == null)
            {
                throw new BaseException(ErrorsMessage.MSG_NOT_EXIST, "Người dùng");
            }

            if (user.IsSuperAdmin == false)
            {
                throw new BaseException("Khách hàng không có quyền tạo thông báo!");
            }

            if (request.Title.Length > 120)
            {
                throw new BaseException(ErrorsMessage.MSG_NOT_VALIDATE, "Tiêu đề");
            }

            if (request.Content.Length > 500)
            {
                throw new BaseException(ErrorsMessage.MSG_NOT_VALIDATE, "Nội dung");
            }

            //Tạo mới thông báo
            var notification = new Notification(request.Title, "", request.Content, request.UserId);

            if (request.ImageFile != null && request.ImageFile.Length > 0)
            {

                var uploadResult = await _cloudService.UploadPhotoAsync(request.ImageFile, "notification");

                if (uploadResult != null)
                {
                    var avatar = new Avatar(uploadResult.PublicId, uploadResult.Url);
                    _avatar.Add(avatar);

                    await _unitOfWork.SaveChangesAsync();

                    notification.Image = avatar.ImageUrl;
                }
                else
                {
                    throw new BaseException("Không thể tải lên hình ảnh!");
                }
            }

            _notiRep.Add(notification);

            await _unitOfWork.SaveChangesAsync();

            return notification.Id;
        }
    }
}
