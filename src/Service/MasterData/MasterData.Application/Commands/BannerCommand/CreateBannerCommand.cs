using Core.Exceptions;
using Core.Infrastructure.Handlers;
using Core.Interfaces.Database;
using Core.Properties;
using Core.SeedWork.Repository;
using Infrastructure.AggregatesModel.MasterData.BannerAggregate;
using Infrastructure.AggregatesModel.MasterData.ImageAggregate;
using MasterData.Application.DTOs.Banner;
using MasterData.Application.Services.CloudinaryService;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace MasterData.Application.Commands.BannerCommand
{
    public class CreateBannerCommand : IRequest<bool>
    {
        public long? BannerId { get; set; }
        public string Title { get; set; }
        public IFormFile ImageFile { get; set; }
        public string Type { get; set; }

    }
    public class CreateBannerCommandHandler : BaseHandler, IRequestHandler<CreateBannerCommand, bool>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public readonly IRepository<Banner> _banRep;
        public readonly IUnitOfWork _unitOfWork;
        private readonly ICloudPhotoService _cloudService;
        private readonly IRepository<Avatar> _avatar;

        public CreateBannerCommandHandler(IHttpContextAccessor httpContextAccessor, IRepository<Banner> banRep, IUnitOfWork unitOfWork, ICloudPhotoService cloudService, IRepository<Avatar> avatar)
        {
            _httpContextAccessor = httpContextAccessor;
            _banRep = banRep;
            _unitOfWork = unitOfWork;
            _cloudService = cloudService;
            _avatar = avatar;
        }
        public async Task<bool> Handle(CreateBannerCommand request, CancellationToken cancellationToken)
        {

            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext.Request.Method == HttpMethods.Post && (request.BannerId == null || request.BannerId == 0))
            {

                if (request.Type != "Mobile" && request.Type != "Web")
                {
                    throw new BaseException("Loại banner không hợp lệ! Vui lòng chọn 'Mobile' hoặc 'Web'.");
                }

                var isTitle = await _banRep.GetAny(e => e.Title.Trim().ToLower() == request.Title.Trim().ToLower());

                if (isTitle)
                {
                    throw new BaseException(ErrorsMessage.MSG_EXIST, "Banner");
                }


                //Tạo mới 1 banner
                var banner = new Banner(request.Title, "", request.Type);

                if (request.ImageFile != null && request.ImageFile.Length > 0)
                {

                    var uploadResult = await _cloudService.UploadPhotoAsync(request.ImageFile, "post");

                    if (uploadResult != null)
                    {
                        var avatar = new Avatar(uploadResult.PublicId, uploadResult.Url);
                        _avatar.Add(avatar);

                        await _unitOfWork.SaveChangesAsync();

                        banner.Image = avatar.ImageUrl;
                    }
                    else
                    {
                        throw new BaseException("Không thể tải lên hình ảnh!");
                    }
                }

                _banRep.Add(banner);

                await _unitOfWork.SaveChangesAsync();


                return true;
            }
            else
            {
                var banner = await _banRep.FindOneAsync(e => e.Id == request.BannerId);
                if (banner == null)
                {
                    throw new BaseException("Không tìm thấy banner");
                }
                if (string.IsNullOrEmpty(request.Title))
                {
                    throw new BaseException(ErrorsMessage.MSG_NOT_EXIST, "Vui lòng không bỏ trống tiêu đề");
                }
                if (request.ImageFile != null && request.ImageFile.Length > 0)
                {

                    var uploadResult = await _cloudService.UploadPhotoAsync(request.ImageFile, "post");

                    if (uploadResult != null)
                    {
                        var avatar = new Avatar(uploadResult.PublicId, uploadResult.Url);
                        _avatar.Add(avatar);

                        await _unitOfWork.SaveChangesAsync();

                        banner.Image = avatar.ImageUrl;
                    }
                    else
                    {
                        throw new BaseException("Không thể tải lên hình ảnh!");
                    }
                }

                banner.Title = request.Title;
                banner.Type = request.Type;
                banner.UpdatedDate = DateTime.Now;
                _banRep.Update(banner);
                await _unitOfWork.SaveChangesAsync();
                return true;



            }
        }
    }
}
