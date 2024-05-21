using Core.Infrastructure.Controllers;
using Core.Models;
using Core.Properties;
using MasterData.Application.Commands.LocationCommand;
using MasterData.Application.Commands.StatusCommand;
using MasterData.Application.DTOs.Location;
using MasterData.Application.DTOs.Status;
using MasterData.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using static MasterData.API.Commons.Routes;

namespace MasterData.API.Controllers
{
    [Route(LocationRoutes.Prefix)]
    public class MapLocationController : BaseApiController
    {

        private readonly IMediator _mediator;
        private readonly ILocationQuery _query;

        public MapLocationController(IMediator mediator, ILocationQuery query)
        {
            _mediator = mediator;
            _query = query;
        }

        /// <summary>
        /// Danh sách location
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpGet(LocationRoutes.List)]
        [ProducesResponseType(typeof(ApiSuccessResult<IList<LocationResponse>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> ListLocation([FromQuery] ListLocationCommand command)
        {
            var response = await _query.ListAsync(command);

            return Ok(new ApiSuccessResult<IList<LocationResponse>>
            {
                Data = response.Data,
                Paging = response.Paging,
                Message = null
            });
        }

        /// <summary>
        /// Tạo mới vị trí
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost(LocationRoutes.Create)]
        [ProducesResponseType(typeof(ApiSuccessResult<LocationResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> CreateLocation([FromBody] CreateLocationCommand command)
        {
            var response = await _mediator.Send(command);

            return Ok(new ApiSuccessResult<LocationResponse>(
                data: response,
                message: string.Format(SuccessMessage.MSG_CREATE_SUCCESS, "Vị trí")));
        }

        /// <summary>
        /// Chỉnh sửa vị trí
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut(LocationRoutes.Update)]
        [ProducesResponseType(typeof(ApiSuccessResult<LocationResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateLocation([FromBody] UpdateLocationCommand command)
        {
            var response = await _mediator.Send(command);

            return Ok(new ApiSuccessResult<LocationResponse>(
                data: response,
                message: string.Format(SuccessMessage.MSG_UPDATE_SUCCESS, "Vị trí")));
        }

        /// <summary>
        /// Xóa vị trí
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpDelete(LocationRoutes.Delete)]
        [ProducesResponseType(typeof(ApiSuccessResult<bool>), StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteLocation([FromBody] DeleteLocationCommand command)
        {
            var response = await _mediator.Send(command);

            return Ok(new ApiSuccessResult<bool>(
                data: response,
                message: string.Format(SuccessMessage.MSG_DELETE_SUCCESS, "Vị trí")));
        }
    }
}
