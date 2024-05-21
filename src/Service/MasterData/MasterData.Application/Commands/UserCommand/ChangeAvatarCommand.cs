using Core.Exceptions;
using Core.Infrastructure.Handlers;
using Core.Interfaces.Database;
using Core.Models.Settings;
using Core.Properties;
using Core.SeedWork.Repository;
using Infrastructure.AggregatesModel.Authen;
using Infrastructure.AggregatesModel.Authen.AccountAggregate;
using Infrastructure.AggregatesModel.MasterData.ImageAggregate;
using Infrastructure.Repositories;
using Infrastructure.Services;
using MasterData.Application.Commands.AccountCommand;
using MasterData.Application.Commands.UserCommand;
using MasterData.Application.Services.CloudinaryService;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileService = Core.Infrastructure.Services.IFileService;

namespace MasterData.Application.Commands.UserCommand
{
    public class ChangeAvatarCommand : IRequest<bool>
    {
        public IFormFile AvatarFile { get; set; }
    }

    public class ChangeUserAvatarCommandHandle : BaseHandler, IRequestHandler<ChangeAvatarCommand, bool>
    {
        private readonly IRepository<User> _userRep;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFileService _fileService;
        private readonly IRepository<AuthenMedia> _media;
        private readonly ICloudPhotoService _cloudService;
        private readonly IRepository<Avatar> _avatar;

        public ChangeUserAvatarCommandHandle(IRepository<User> userRep, IUnitOfWork unitOfWork, IFileService fileService, IRepository<AuthenMedia> media, IRepository<Avatar> avatar, ICloudPhotoService cloudService)
        {
            _userRep = userRep;
            _unitOfWork = unitOfWork;
            _fileService = fileService;
            _media = media;
            _avatar = avatar;
            _cloudService = cloudService;
        }

        public async Task<bool> Handle(ChangeAvatarCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRep.FindOneAsync(e => e.Id == UserId);

            if (user == null)
            {
                throw new BaseException(ErrorsMessage.MSG_NOT_EXIST, "Người dùng");
            }

            if (request.AvatarFile != null && request.AvatarFile.Length > 0)
            {
                var uploadResult = await _cloudService.UploadPhotoAsync(request.AvatarFile, "profile");

                if (uploadResult != null)
                {
                    var avatar = new Avatar(uploadResult.PublicId, uploadResult.Url);
                    _avatar.Add(avatar);

                    await _unitOfWork.SaveChangesAsync();

                    user.PhotoId = avatar.Id;
                }
                else
                {
                    throw new BaseException("Không thể tải lên hình ảnh!");
                }
            }

            // Lưu thông tin người dùng đã cập nhật vào cơ sở dữ liệu
            _userRep.Update(user);

            await _unitOfWork.SaveChangesAsync();

            // Trả về true để biểu thị rằng quá trình thay đổi avatar thành công
            return true;
        }
    }
}
