using Core.Infrastructure.Controllers;
using Core.Models;
using Core.Properties;
using MasterData.Application.Commands.EventCommand;
using MasterData.Application.Commands.PostCommand;
using MasterData.Application.DTOs.Event;
using MasterData.Application.DTOs.Post;
using MasterData.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using static MasterData.API.Commons.Routes;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace MasterData.API.Controllers
{
    [Route(EventRoutes.Prefix)]
    public class EventController : BaseApiController
    {
        private readonly IMediator _mediator;
        private readonly IEventQuery _query;

        public EventController(IMediator mediator, IEventQuery query)
        {
            _mediator = mediator;
            _query = query;
        }
        /// <summary>
        ///  Tạo 1 sự kiện
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost(EventRoutes.Create)]
        [ProducesResponseType(typeof(ApiSuccessResult<bool>), StatusCodes.Status200OK)]
        public async Task<IActionResult> CreatePost([FromForm] CreateEventCommand command)
        {
            var response = await _mediator.Send(command);

            return Ok(new ApiSuccessResult<bool>(
                data: response,
                message: string.Format(SuccessMessage.MSG_CREATE_SUCCESS, "Sự kiện")));
        }

        /// <summary>
        /// Chỉnh sửa sự kiện
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut(EventRoutes.Create)]
        [ProducesResponseType(typeof(ApiSuccessResult<bool>), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateP([FromForm] CreateEventCommand command)
        {
            var response = await _mediator.Send(command);

            return Ok(new ApiSuccessResult<bool>(
                data: response,
                message: string.Format(SuccessMessage.MSG_UPDATE_SUCCESS, "Sự kiện")));
        }



        /// <summary>
        /// Chi tiết thông tin 1 sự kiện
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpGet(EventRoutes.Detail)]
        [ProducesResponseType(typeof(ApiSuccessResult<EventDetailResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetEvent([FromQuery] GetEventCommand command)
        {
            var response = await _query.GetAsync(command);

            return Ok(new ApiSuccessResult<EventDetailResponse>(
                data: response));
        }


        /// <summary>
        /// Danh sách các sự kiện
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpGet(EventRoutes.List)]
        [ProducesResponseType(typeof(ApiSuccessResult<IList<EventResponse>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> ListBike([FromQuery] ListEventCommand command)
        {
            var response = await _query.ListAsync(command);

            return Ok(new ApiSuccessResult<IList<EventResponse>>
            {
                Data = response.Data,
                Paging = response.Paging,
                Message = null
            });
        }

        /// <summary>
        /// Xóa 1 sự kiện
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>

        [HttpDelete(PostRoutes.Delete)]
        [ProducesResponseType(typeof(ApiSuccessResult<bool>), StatusCodes.Status200OK)]
        public async Task<IActionResult> DeletePost(DeleteEventCommand command)
        {
            var response = await _mediator.Send(command);

            return Ok(new ApiSuccessResult<bool>(
                data: response,
                message: string.Format(SuccessMessage.MSG_DELETE_SUCCESS, "Sự kiện")));
        }




    }
}
