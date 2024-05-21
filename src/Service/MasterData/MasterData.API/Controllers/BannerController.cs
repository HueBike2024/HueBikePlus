using Core.Infrastructure.Controllers;
using Core.Models;
using Core.Properties;
using MasterData.Application.Commands.BannerCommand;
using MasterData.Application.Commands.PostCommand;
using MasterData.Application.DTOs.Banner;
using MasterData.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using static MasterData.API.Commons.Routes;

namespace MasterData.API.Controllers
{
    [Route(BannerRoutes.Prefix)]
    public class BannerController : BaseApiController
    {
        private readonly IMediator _mediator;
        private readonly IBannerQuery _query;

        public BannerController(IMediator mediator, IBannerQuery query)
        {
            _mediator = mediator;
            _query = query;
        }
        /// <summary>
        ///  Tạo 1 banner 
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost(BannerRoutes.Create)]
        [ProducesResponseType(typeof(ApiSuccessResult<bool>), StatusCodes.Status200OK)]
        public async Task<IActionResult> CreateBannerWeb([FromForm] CreateBannerCommand command)
        {
            //command.Type = "Web";
            var response = await _mediator.Send(command);

            return Ok(new ApiSuccessResult<bool>(
                data: response,
                message: string.Format(SuccessMessage.MSG_CREATE_SUCCESS, "banner")));
        }

        /// <summary>
        /// Chỉnh sửa banner
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut(PostRoutes.Create)]
        [ProducesResponseType(typeof(ApiSuccessResult<bool>), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateP([FromForm] CreateBannerCommand command)
        {
            var response = await _mediator.Send(command);

            return Ok(new ApiSuccessResult<bool>(
                data: response,
                message: string.Format(SuccessMessage.MSG_UPDATE_SUCCESS, "Banner")));
        }
        ///// <summary>
        ///// tạo 1 banner mobile
        ///// </summary>
        ///// <param name="command"></param>
        ///// <returns></returns>
        //[HttpPost(BannerRoutes.MobileCreate)]
        //[ProducesResponseType(typeof(ApiSuccessResult<BannerResponse>), StatusCodes.Status200OK)]
        //public async Task<IActionResult> CreateBannerMobile([FromForm] CreateBannerCommand command)
        //{
        //    command.Type = "Mobile";
        //    var response = await _mediator.Send(command);

        //    return Ok(new ApiSuccessResult<BannerResponse>(
        //        data: response,
        //        message: string.Format(SuccessMessage.MSG_CREATE_SUCCESS, "banner")));
        //}



        /// <summary>
        /// Chi tiết thông tin 1 banner
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpGet(PostRoutes.Detail)]
        [ProducesResponseType(typeof(ApiSuccessResult<BannerDetailResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetBike([FromQuery] GetBannerCommand command)
        {
            var response = await _query.GetAsync(command);

            return Ok(new ApiSuccessResult<BannerDetailResponse>(
                data: response));
        }

        /// <summary>
        /// danh sách banner web
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpGet(BannerRoutes.WebList)]
        [ProducesResponseType(typeof(ApiSuccessResult<IList<BannerResponse>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> ListBannersWeb([FromQuery] ListBannerCommand command)
        {
            command.Type = "Web";
            var response = await _query.ListAsync(command);

            return Ok(new ApiSuccessResult<IList<BannerResponse>>
            {
                Data = response.Data,
                Paging = response.Paging,
                Message = null
            });
        }
        /// <summary>
        /// danh sách banner mobile
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpGet(BannerRoutes.MobileList)]
        [ProducesResponseType(typeof(ApiSuccessResult<IList<BannerResponse>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> ListBannersMobile([FromQuery] ListBannerCommand command)
        {
            command.Type = "Mobile";
            var response = await _query.ListAsync(command);

            return Ok(new ApiSuccessResult<IList<BannerResponse>>
            {
                Data = response.Data,
                Paging = response.Paging,
                Message = null
            });
        }

        /// <summary>
        /// Xóa 1 banner
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>

        [HttpDelete(PostRoutes.Delete)]
        [ProducesResponseType(typeof(ApiSuccessResult<bool>), StatusCodes.Status200OK)]
        public async Task<IActionResult> DeletePost(DeleteBannerCommand command)
        {
            var response = await _mediator.Send(command);

            return Ok(new ApiSuccessResult<bool>(
                data: response,
                message: string.Format(SuccessMessage.MSG_DELETE_SUCCESS, "Banner")));
        }


    }
}
