using Core.Infrastructure.Controllers;
using Core.Models;
using Core.Properties;
using MasterData.Application.Commands.StationCommand;
using MasterData.Application.DTOs.Station;
using MasterData.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using static MasterData.API.Commons.Routes;

namespace MasterData.API.Controllers
{
    [Route(StationRoutes.Prefix)]
    public class StationController : BaseApiController
    {

        private readonly IMediator _mediator;
        private readonly IStationQuery _query;

        public StationController(IMediator mediator, IStationQuery query)
        {
            _mediator = mediator;
            _query = query;
        }

        /// <summary>
        /// Danh sách trạm
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpGet(StationRoutes.List)]
        [ProducesResponseType(typeof(ApiSuccessResult<IList<StationResponse>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> ListStation([FromQuery] ListStationCommand command)
        {
            var response = await _query.ListAsync(command);

            return Ok(new ApiSuccessResult<IList<StationResponse>>
            {
                Data = response.Data,
                Paging = response.Paging,
                Message = null
            });
        }

        /// <summary>
        /// Danh sách trạm theo thứ tự gần đến xa
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpGet(StationRoutes.ListStationtoUser)]
        [ProducesResponseType(typeof(ApiSuccessResult<IList<StationResponse>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> ListStationtoUser([FromQuery] ListStationUserCommand command)
        {
            var response = await _query.ListStation(command);

            return Ok(new ApiSuccessResult<IList<StationResponse>>
            {
                Data = response.Data,
                Paging = response.Paging,
                Message = null
            });
        }


        /// <summary>
        /// Chi tiết thông tin 1 trạm
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpGet(StationRoutes.Detail)]
        [ProducesResponseType(typeof(ApiSuccessResult<StationDetailResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetStation([FromQuery] GetStationCommand command)
        {
            var response = await _query.GetAsync(command);

            return Ok(new ApiSuccessResult<StationDetailResponse>(
                data: response));
        }
        /// <summary>
        /// Tạo 1 trạm mới
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost(StationRoutes.Create)]
        [ProducesResponseType(typeof(ApiSuccessResult<bool>), StatusCodes.Status200OK)]
        public async Task<IActionResult> CreateStation([FromBody] CreateStationCommand command)
        {
            var response = await _mediator.Send(command);

            return Ok(new ApiSuccessResult<bool>(
                data: response,
                message: string.Format(SuccessMessage.MSG_CREATE_SUCCESS, "Trạm")));
        }

        /// <summary>
        /// Chỉnh sửa trạm
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut(StationRoutes.Create)]
        [ProducesResponseType(typeof(ApiSuccessResult<bool>), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateStation([FromBody] CreateStationCommand command)
        {
            var response = await _mediator.Send(command);

            return Ok(new ApiSuccessResult<bool>(
                data: response,
                message: string.Format(SuccessMessage.MSG_UPDATE_SUCCESS, "Trạm")));
        }

        /// <summary>
        /// Xóa trạm
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpDelete(StationRoutes.Delete)]
        [ProducesResponseType(typeof(ApiSuccessResult<bool>), StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteStation([FromBody] DeleteStationCommand command)
        {
            var response = await _mediator.Send(command);

            return Ok(new ApiSuccessResult<bool>(
                data: response,
                message: string.Format(SuccessMessage.MSG_DELETE_SUCCESS, "Trạm")));
        }
    }
}
