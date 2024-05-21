using Core.Infrastructure.Controllers;
using Core.Models;
using Core.Properties;
using MasterData.Application.Commands.PostCommand;
using MasterData.Application.Commands.StationCommand;
using MasterData.Application.DTOs.Post;
using MasterData.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using static MasterData.API.Commons.Routes;

namespace MasterData.API.Controllers
{
    [Route(PostRoutes.Prefix)]
    public class PostController : BaseApiController
    {
        private readonly IMediator _mediator;
        private readonly IPostQuery _query;

        public PostController(IMediator mediator, IPostQuery query)
        {
            _mediator = mediator;
            _query = query;
        }
        /// <summary>
        ///  Tạo 1 bài viết/sự kiện
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost(PostRoutes.Create)]
        [ProducesResponseType(typeof(ApiSuccessResult<bool>), StatusCodes.Status200OK)]
        public async Task<IActionResult> CreatePost([FromForm] CreatePostCommand command)
        {
            var response = await _mediator.Send(command);

            return Ok(new ApiSuccessResult<bool>(
                data: response,
                message: string.Format(SuccessMessage.MSG_CREATE_SUCCESS, "Bài viết")));
        }

        /// <summary>
        /// Chỉnh sửa bài viết/ sự kiện
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut(PostRoutes.Create)]
        [ProducesResponseType(typeof(ApiSuccessResult<bool>), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateP([FromForm] CreatePostCommand command)
        {
            var response = await _mediator.Send(command);

            return Ok(new ApiSuccessResult<bool>(
                data: response,
                message: string.Format(SuccessMessage.MSG_UPDATE_SUCCESS, "Bài viết")));
        }



        /// <summary>
        /// Chi tiết thông tin 1 bài viết/sự kiện
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpGet(PostRoutes.Detail)]
        [ProducesResponseType(typeof(ApiSuccessResult<PostDetailResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetBike([FromQuery] GetPostCommand command)
        {
            var response = await _query.GetAsync(command);

            return Ok(new ApiSuccessResult<PostDetailResponse>(
                data: response));
        }


        /// <summary>
        /// Danh sách bài viết/sự kiện
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpGet(PostRoutes.List)]
        [ProducesResponseType(typeof(ApiSuccessResult<IList<PostResponse>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> ListBike([FromQuery] ListPostCommand command)
        {
            var response = await _query.ListAsync(command);

            return Ok(new ApiSuccessResult<IList<PostResponse>>
            {
                Data = response.Data,
                Paging = response.Paging,
                Message = null
            });
        }

        /// <summary>
        /// Xóa 1 bai viet/sự kiện
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>

        [HttpDelete(PostRoutes.Delete)]
        [ProducesResponseType(typeof(ApiSuccessResult<bool>), StatusCodes.Status200OK)]
        public async Task<IActionResult> DeletePost(DeletePostCommand command)
        {
            var response = await _mediator.Send(command);

            return Ok(new ApiSuccessResult<bool>(
                data: response,
                message: string.Format(SuccessMessage.MSG_DELETE_SUCCESS, "Post")));
        }


    }
}
