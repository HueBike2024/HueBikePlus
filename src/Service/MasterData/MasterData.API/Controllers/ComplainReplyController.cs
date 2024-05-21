using Core.Infrastructure.Controllers;
using Core.Models;
using Core.Properties;
using MasterData.Application.Commands.ComplainCommand;
using MasterData.Application.Commands.ComplainReplyCommand;
using MasterData.Application.DTOs.Complain;
using MasterData.Application.DTOs.ComplainReply;
using MasterData.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using static MasterData.API.Commons.Routes;

namespace MasterData.API.Controllers
{
    [Route(ComplainReplyRoutes.Prefix)]
    public class ComplainReplyController : BaseApiController
    {

        private readonly IMediator _mediator;

        public ComplainReplyController(IMediator mediator)
        {
            _mediator = mediator;
        }
        /// <summary>
        /// Tạo mới complain reply
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost(ComplainReplyRoutes.Create)]
        [ProducesResponseType(typeof(ApiSuccessResult<ComplainReplyResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> CreateReply([FromBody] CreateComplainReplyCommand command)
        {
            var response = await _mediator.Send(command);

            return Ok(new ApiSuccessResult<ComplainReplyResponse>(
                data: response,
                message: string.Format(SuccessMessage.MSG_CREATE_SUCCESS, "Câu trả lời")));
        }

        /// <summary>
        /// Sửa đổi nội dung complain reply
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut(ComplainReplyRoutes.UpdateContent)]
        [ProducesResponseType(typeof(ApiSuccessResult<bool>), StatusCodes.Status200OK)]
        public async Task<IActionResult> ChangeContentReply([FromBody] ChangeContentReplyCommand command)
        {
            var response = await _mediator.Send(command);

            return Ok(new ApiSuccessResult<bool>(
                data: response,
                message: string.Format(SuccessMessage.MSG_UPDATE_SUCCESS, "Câu trả lời")));
        }

        /// <summary>
        /// xóa 1 complain reply
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpDelete(ComplainReplyRoutes.Delete)]
        [ProducesResponseType(typeof(ApiSuccessResult<bool>), StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteReply([FromBody] DeleteComplainReplyCommand command)
        {
            var response = await _mediator.Send(command);

            return Ok(new ApiSuccessResult<bool>(
                data: response,
                message: string.Format(SuccessMessage.MSG_DELETE_SUCCESS, "Câu trả lời")));
        }
    }
}
