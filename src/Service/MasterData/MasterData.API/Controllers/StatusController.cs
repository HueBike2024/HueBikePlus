using Core.Infrastructure.Controllers;
using Core.Models;
using Core.Properties;
using MasterData.Application.Commands.ComplainCommand;
using MasterData.Application.Commands.StatusCommand;
using MasterData.Application.DTOs.Complain;
using MasterData.Application.DTOs.Status;
using MasterData.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using static MasterData.API.Commons.Routes;

namespace MasterData.API.Controllers
{
    [Route(StatusRoutes.Prefix)]
    public class StatusController : BaseApiController
    {

        private readonly IMediator _mediator;
        private readonly IStatusQuery _query;

        public StatusController(IMediator mediator, IStatusQuery query)
        {
            _mediator = mediator;
            _query = query;
        }

        /// <summary>
        /// Danh sách status
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpGet(StatusRoutes.List)]
        [ProducesResponseType(typeof(ApiSuccessResult<IList<StatusResponse>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> ListStatus([FromQuery] ListStatusCommand command)
        {
            var response = await _query.ListAsync(command);

            return Ok(new ApiSuccessResult<IList<StatusResponse>>
            {
                Data = response.Data,
                Paging = response.Paging,
                Message = null
            });
        }

        /// <summary>
        /// Tạo mới trạng thái
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost(StatusRoutes.Create)]
        [ProducesResponseType(typeof(ApiSuccessResult<StatusResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> CreateStatus([FromBody] CreateStatusCommand command)
        {
            var response = await _mediator.Send(command);

            return Ok(new ApiSuccessResult<StatusResponse>(
                data: response,
                message: string.Format(SuccessMessage.MSG_CREATE_SUCCESS, "Trạng thái")));
        }

        /// <summary>
        /// Chỉnh sửa trạng thái
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut(StatusRoutes.Update)]
        [ProducesResponseType(typeof(ApiSuccessResult<StatusResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateStatus([FromBody] UpdateStatusCommand command)
        {
            var response = await _mediator.Send(command);

            return Ok(new ApiSuccessResult<StatusResponse>(
                data: response,
                message: string.Format(SuccessMessage.MSG_UPDATE_SUCCESS, "Trạng thái")));
        }

        /// <summary>
        /// Xóa trạng thái
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpDelete(StatusRoutes.Delete)]
        [ProducesResponseType(typeof(ApiSuccessResult<bool>), StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteStatus([FromBody] DeleteStatusCommand command)
        {
            var response = await _mediator.Send(command);

            return Ok(new ApiSuccessResult<bool>(
                data: response,
                message: string.Format(SuccessMessage.MSG_DELETE_SUCCESS, "Trạng thái")));
        }
    }
}
