using Core.Infrastructure.Controllers;
using Core.Models;
using Core.Properties;
using MasterData.Application.Commands.ComplainCommand;
using MasterData.Application.DTOs.Complain;
using MasterData.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using static MasterData.API.Commons.Routes;

namespace MasterData.API.Controllers
{
    [Route(ComplainRoutes.Prefix)]
    public class ComplainController : BaseApiController
    {
        private readonly IMediator _mediator;
        private readonly IComplainQuery _query;

        public ComplainController(IMediator mediator, IComplainQuery query)
        {
            _mediator = mediator;
            _query = query;
        }

        /// <summary>
        /// Danh sách complain
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpGet(ComplainRoutes.List)]
        [ProducesResponseType(typeof(ApiSuccessResult<IList<ComplainResponse>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> ListComplain([FromQuery] ListComplainCommand command)
        {
            var response = await _query.ListAsync(command);

            return Ok(new ApiSuccessResult<IList<ComplainResponse>>
            {
                Data = response.Data,
                Paging = response.Paging,
                Message = null
            });
        }

        /// <summary>
        /// Thông tin chi tiết 1 complain
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpGet(ComplainRoutes.Detail)]
        [ProducesResponseType(typeof(ApiSuccessResult<ComplainDetailResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetComplainDetail([FromQuery] DetailComplainCommand command)
        {
            var response = await _query.GetAsync(command);

            return Ok(new ApiSuccessResult<ComplainDetailResponse>(
                data: response));
        }

        /// <summary>
        /// Tạo mới complain
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost(ComplainRoutes.Create)]
        [ProducesResponseType(typeof(ApiSuccessResult<ComplainResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> CreateComplain([FromForm] CreateComplainCommand command)
        {
            var response = await _mediator.Send(command);

            return Ok(new ApiSuccessResult<ComplainResponse>(
                data: response,
                message: string.Format(SuccessMessage.MSG_CREATE_SUCCESS, "Khiếu nại")));
        }

        /// <summary>
        /// Xóa 1 complain
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpDelete(ComplainRoutes.Delete)]
        [ProducesResponseType(typeof(ApiSuccessResult<bool>), StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteComplain([FromBody] DeleteComplainCommand command)
        {
            var response = await _mediator.Send(command);

            return Ok(new ApiSuccessResult<bool>(
                data: response,
                message: string.Format(SuccessMessage.MSG_DELETE_SUCCESS, "Khiếu nại")));
        }
    }
}
