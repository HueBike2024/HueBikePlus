using Core.Exceptions;
using Core.Infrastructure.Handlers;
using Core.Interfaces.Database;
using Core.Properties;
using Core.SeedWork.Repository;
using Infrastructure.AggregatesModel.MasterData.ImageAggregate;
using Infrastructure.AggregatesModel.MasterData.PostAggregate;
using MasterData.Application.Services.CloudinaryService;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace MasterData.Application.Commands.PostCommand
{
    public class CreatePostCommand : IRequest<bool>
    {
        public long? PostId { get; set; }
        public string Title { get; set; }
        //public string Image { get; set; }
        public IFormFile ImageFile { get; set; }
        public string Content { get; set; }
    }
    public class CreatePostCommandHandler : BaseHandler, IRequestHandler<CreatePostCommand, bool>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public readonly IRepository<Post> _postRep;
        public readonly IUnitOfWork _unitOfWork;
        private readonly ICloudPhotoService _cloudService;
        private readonly IRepository<Avatar> _avatar;

        public CreatePostCommandHandler(IHttpContextAccessor httpContextAccessor, IRepository<Post> postRep, IUnitOfWork unitOfWork, ICloudPhotoService cloudService, IRepository<Avatar> avatar)
        {
            _httpContextAccessor = httpContextAccessor;
            _postRep = postRep;
            _unitOfWork = unitOfWork;
            _cloudService = cloudService;
            _avatar = avatar;
        }
        public async Task<bool> Handle(CreatePostCommand request, CancellationToken cancellationToken)
        {
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext.Request.Method == HttpMethods.Post && (request.PostId == null || request.PostId == 0))
            {


                var isTitle = await _postRep.GetAny(e => e.Title.Trim().ToLower() == request.Title.Trim().ToLower());

                if (isTitle)
                {
                    throw new BaseException(ErrorsMessage.MSG_EXIST, "Bài viết");
                }


                //Tạo mới 1 bài viết/Tin tức
                var post = new Post(request.Title, "", request.Content);

                if (request.ImageFile != null && request.ImageFile.Length > 0)
                {

                    var uploadResult = await _cloudService.UploadPhotoAsync(request.ImageFile, "post");

                    if (uploadResult != null)
                    {
                        var avatar = new Avatar(uploadResult.PublicId, uploadResult.Url);
                        _avatar.Add(avatar);

                        await _unitOfWork.SaveChangesAsync();

                        post.Image = avatar.ImageUrl;
                    }
                    else
                    {
                        throw new BaseException("Không thể tải lên hình ảnh!");
                    }
                }



                _postRep.Add(post);

                await _unitOfWork.SaveChangesAsync();

                //var postResponse = new PostResponse
                //{
                //    PostId = post.Id,
                //    Tilte = post.Title,
                //    Image = post.Image,
                //    Content = post.Content,
                //    CreatedDate = post.CreatedDate,
                //};
                return true;
                //return postResponse;
            }
            else
            {
                var post = await _postRep.FindOneAsync(e => e.Id == request.PostId);
                if (post == null)
                {
                    throw new BaseException("Không tìm thấy bài viết hoặc tin tức!");
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

                        post.Image = avatar.ImageUrl;
                    }
                    else
                    {
                        throw new BaseException("Không thể tải lên hình ảnh!");
                    }
                }

                post.Title = request.Title;
                post.Content = request.Content;              
                post.UpdatedDate = DateTime.Now;

                _postRep.Update(post);

                await _unitOfWork.SaveChangesAsync();
                return true;

            }
        }
    }

}
