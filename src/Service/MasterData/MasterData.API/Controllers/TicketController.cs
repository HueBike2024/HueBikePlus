using Core.Infrastructure.Controllers;
using Core.Models;
using Core.Properties;
using MasterData.Application.Commands.ComplainCommand;
using MasterData.Application.Commands.NotificationCommand;
using MasterData.Application.Commands.TicketCommand;
using MasterData.Application.DTOs.Complain;
using MasterData.Application.DTOs.Notification;
using MasterData.Application.DTOs.Ticket;
using MasterData.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using static MasterData.API.Commons.Routes;

namespace MasterData.API.Controllers
{
    [Route(TicketRoutes.Prefix)]
    public class TicketController : BaseApiController
    {

        private readonly IMediator _mediator;
        private readonly ITicketQuery _query;

        public TicketController(IMediator mediator, ITicketQuery query)
        {
            _mediator = mediator;
            _query = query;
        }

        /// <summary>
        /// Danh sách tất cả vé
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpGet(TicketRoutes.ListAll)]
        [ProducesResponseType(typeof(ApiSuccessResult<IList<ListTicketResponse>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> ListSended([FromQuery] ListTicketCommand command)
        {
            var response = await _query.AllTicketListAsync(command);

            return Ok(new ApiSuccessResult<IList<ListTicketResponse>>
            {
                Data = response.Data,
                Paging = response.Paging,
                Message = null
            });
        }

        /// <summary>
        /// Danh sách vé đã mua
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpGet(TicketRoutes.ListPurchaed)]
        [ProducesResponseType(typeof(ApiSuccessResult<IList<ListTicketResponse>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> ListSended([FromQuery] ListPurchasedTicketCommand command)
        {
            var response = await _query.PurchasedListAsync(command);

            return Ok(new ApiSuccessResult<IList<ListTicketResponse>>
            {
                Data = response.Data,
                Paging = response.Paging,
                Message = null
            });
        }

        /// <summary>
        /// Thông tin chi tiết 1 vé
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpGet(TicketRoutes.Detail)]
        [ProducesResponseType(typeof(ApiSuccessResult<TicketResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetNotificationDetail([FromQuery] TicketDetailCommand command)
        {
            var response = await _query.GetAsync(command);

            return Ok(new ApiSuccessResult<TicketResponse>(
                data: response));
        }

        /// <summary>
        /// Đặt trước vé
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost(TicketRoutes.preBook)]
        [ProducesResponseType(typeof(ApiSuccessResult<TicketResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> PreBookingTicket([FromForm] PreBookTicketCommand command)
        {
            var response = await _mediator.Send(command);

            return Ok(new ApiSuccessResult<TicketResponse>(
                data: response,
                message: string.Format("Đặt vé thành công!")));
        }

        /// <summary>
        /// Chỉnh sửa thông tin đặt vé
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut(TicketRoutes.ChangeInfo)]
        [ProducesResponseType(typeof(ApiSuccessResult<TicketResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> ChangeBookingInfo([FromForm] PreBookTicketCommand command)
        {
            var response = await _mediator.Send(command);

            return Ok(new ApiSuccessResult<TicketResponse>(
                data: response,
                message: string.Format("Thay đổi thông tin đặt vé thành công!")));
        }

        /// <summary>
        /// Xóa/ Hủy vé
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpDelete(TicketRoutes.Delete)]
        [ProducesResponseType(typeof(ApiSuccessResult<bool>), StatusCodes.Status200OK)]
        public async Task<IActionResult> CancelTicket([FromBody] DeleteTicketCommand command)
        {
            var response = await _mediator.Send(command);

            return Ok(new ApiSuccessResult<bool>(
                data: response,
                message: string.Format("Hủy vé thành công!")));
        }
    }
}
