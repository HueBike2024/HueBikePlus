using Core.Infrastructure.Controllers;
using Core.Models;
using Core.Properties;
using MasterData.Application.Commands.CategoryTicketCommand;
using MasterData.Application.Commands.StatusCommand;
using MasterData.Application.DTOs.CategoryTicket;
using MasterData.Application.DTOs.Status;
using MasterData.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using static MasterData.API.Commons.Routes;

namespace MasterData.API.Controllers
{
    [Route(CategoryTicketRoutes.Prefix)]
    public class CategoryTicketController : BaseApiController
    {
        private readonly IMediator _mediator;
        private readonly ICategoryTicketQuery _query;

        public CategoryTicketController(IMediator mediator, ICategoryTicketQuery query)
        {
            _mediator = mediator;
            _query = query;
        }

        /// <summary>
        /// Danh sách loại vé
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpGet(StatusRoutes.List)]
        [ProducesResponseType(typeof(ApiSuccessResult<IList<CategoryticketResponse>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> ListCategoryTicket([FromQuery] ListCategoryTicketCommand command)
        {
            var response = await _query.ListAsync(command);

            return Ok(new ApiSuccessResult<IList<CategoryticketResponse>>
            {
                Data = response.Data,
                Paging = response.Paging,
                Message = null
            });
        }

        /// <summary>
        /// Tạo mới loại vé
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost(StatusRoutes.Create)]
        [ProducesResponseType(typeof(ApiSuccessResult<CategoryticketResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> CreateCategoryTicket([FromForm] CreateCategoryTicketCommmand command)
        {
            var response = await _mediator.Send(command);

            return Ok(new ApiSuccessResult<CategoryticketResponse>(
                data: response,
                message: string.Format(SuccessMessage.MSG_CREATE_SUCCESS, "loại vé")));
        }

        /// <summary>
        /// Chỉnh sửa loại vé
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut(StatusRoutes.Update)]
        [ProducesResponseType(typeof(ApiSuccessResult<CategoryticketResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateStatus([FromForm] CreateCategoryTicketCommmand command)
        {
            var response = await _mediator.Send(command);

            return Ok(new ApiSuccessResult<CategoryticketResponse>(
                data: response,
                message: string.Format(SuccessMessage.MSG_UPDATE_SUCCESS, "Loại vé")));
        }

        /// <summary>
        /// Xóa loại vé
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpDelete(StatusRoutes.Delete)]
        [ProducesResponseType(typeof(ApiSuccessResult<bool>), StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteCategoryTicket([FromBody] DeleteCategoryTicketCommmand command)
        {
            var response = await _mediator.Send(command);

            return Ok(new ApiSuccessResult<bool>(
                data: response,
                message: string.Format(SuccessMessage.MSG_DELETE_SUCCESS, "Loại vé")));
        }
    }
}
