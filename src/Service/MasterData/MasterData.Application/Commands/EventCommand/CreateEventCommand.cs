using Core.Exceptions;
using Core.Infrastructure.Handlers;
using Core.Interfaces.Database;
using Core.Properties;
using Core.SeedWork.Repository;
using Infrastructure.AggregatesModel.MasterData.EventAggregate;
using Infrastructure.AggregatesModel.MasterData.ImageAggregate;
using MasterData.Application.Services.CloudinaryService;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace MasterData.Application.Commands.EventCommand
{
    public class CreateEventCommand : IRequest<bool>
    {
        public long? EventId { get; set; }
        public string Title { get; set; }
        //public string Image { get; set; }
        public IFormFile ImageFile { get; set; }
        public string Description { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }
        public string Location { get; set; }
        public string Organizer { get; set; }
    }
    public class CreateEventCommandHandler : BaseHandler, IRequestHandler<CreateEventCommand, bool>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public readonly IRepository<Event> _evtRep;
        public readonly IUnitOfWork _unitOfWork;
        private readonly ICloudPhotoService _cloudService;
        private readonly IRepository<Avatar> _avatar;

        public CreateEventCommandHandler(IHttpContextAccessor httpContextAccessor, IRepository<Event> evtRep, IUnitOfWork unitOfWork, ICloudPhotoService cloudService, IRepository<Avatar> avatar)
        {
            _httpContextAccessor = httpContextAccessor;
            _evtRep = evtRep;
            _unitOfWork = unitOfWork;
            _cloudService = cloudService;
            _avatar = avatar;
        }
        public async Task<bool> Handle(CreateEventCommand request, CancellationToken cancellationToken)
        {
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext.Request.Method == HttpMethods.Post && (request.EventId == null || request.EventId == 0))
            {


                var isTitle = await _evtRep.GetAny(e => e.Title.Trim().ToLower() == request.Title.Trim().ToLower());

                if (isTitle)
                {
                    throw new BaseException(ErrorsMessage.MSG_EXIST, "sự kiện");
                }


                //Tạo mới 1 sự kiện
                var evt = new Event(request.Title, "", request.Description,request.DateStart,request.DateEnd,request.Location,request.Organizer);

                if (request.ImageFile != null && request.ImageFile.Length > 0)
                {

                    var uploadResult = await _cloudService.UploadPhotoAsync(request.ImageFile, "event");

                    if (uploadResult != null)
                    {
                        var avatar = new Avatar(uploadResult.PublicId, uploadResult.Url);
                        _avatar.Add(avatar);

                        await _unitOfWork.SaveChangesAsync();

                        evt.Image = avatar.ImageUrl;
                    }
                    else
                    {
                        throw new BaseException("Không thể tải lên hình ảnh!");
                    }
                }



                _evtRep.Add(evt);

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
                var evt = await _evtRep.FindOneAsync(e => e.Id == request.EventId);
                if (evt == null)
                {
                    throw new BaseException("Không tìm thấy sự kiện!");
                }
                if (string.IsNullOrEmpty(request.Title))
                {
                    throw new BaseException(ErrorsMessage.MSG_NOT_EXIST, "Vui lòng không bỏ trống tiêu đề");
                }

                if (request.ImageFile != null && request.ImageFile.Length > 0)
                {

                    var uploadResult = await _cloudService.UploadPhotoAsync(request.ImageFile, "event");

                    if (uploadResult != null)
                    {
                        var avatar = new Avatar(uploadResult.PublicId, uploadResult.Url);
                        _avatar.Add(avatar);

                        await _unitOfWork.SaveChangesAsync();

                        evt.Image = avatar.ImageUrl;
                    }
                    else
                    {
                        throw new BaseException("Không thể tải lên hình ảnh!");
                    }
                }

                evt.Title = request.Title;
                evt.Description = request.Description;

                evt.UpdatedDate = DateTime.Now;

                _evtRep.Update(evt);

                await _unitOfWork.SaveChangesAsync();
                return true;

            }
        }
    }
}
