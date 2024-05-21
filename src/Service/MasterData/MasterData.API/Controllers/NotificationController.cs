using Core.Infrastructure.Controllers;
using Core.Models;
using Core.Properties;
using MasterData.Application.Commands.ComplainCommand;
using MasterData.Application.Commands.NotificationCommand;
using MasterData.Application.Commands.UnitCommand;
using MasterData.Application.DTOs.Complain;
using MasterData.Application.DTOs.Notification;
using MasterData.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using static MasterData.API.Commons.Routes;

namespace MasterData.API.Controllers
{
    [Route(NotificationRoutes.Prefix)]
    public class NotificationController : BaseApiController
    {
        private readonly IMediator _mediator;
        private readonly INotificationQuery _query;

        public NotificationController(IMediator mediator, INotificationQuery query)
        {
            _mediator = mediator;
            _query = query;
        }

        /// <summary>
        /// Danh sách thông báo gửi đi
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpGet(NotificationRoutes.ListSended)]
        [ProducesResponseType(typeof(ApiSuccessResult<IList<NotificationSendedResponse>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> ListSended([FromQuery] ListNotificationSendedCommand command)
        {
            var response = await _query.SendedListAsync(command);

            return Ok(new ApiSuccessResult<IList<NotificationSendedResponse>>
            {
                Data = response.Data,
                Paging = response.Paging,
                Message = null
            });
        }

        /// <summary>
        /// Danh sách thông báo nhận được
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpGet(NotificationRoutes.ListReceived)]
        [ProducesResponseType(typeof(ApiSuccessResult<IList<NotificationReceivedResponse>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> ListReceived([FromQuery] ListNotificationReceivedCommand command)
        {
            var response = await _query.ReceivedListAsync(command);

            return Ok(new ApiSuccessResult<IList<NotificationReceivedResponse>>
            {
                Data = response.Data,
                Paging = response.Paging,
                Message = null
            });
        }

        /// <summary>
        /// Thông tin chi tiết 1 thông báo
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpGet(NotificationRoutes.Detail)]
        [ProducesResponseType(typeof(ApiSuccessResult<NotificationDetailResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetNotificationDetail([FromQuery] NotificationDetailCommand command)
        {
            var response = await _query.GetAsync(command);

            return Ok(new ApiSuccessResult<NotificationDetailResponse>(
                data: response));
        }

        /// <summary>
        /// Tạo mới thông báo
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost(NotificationRoutes.Create)]
        [ProducesResponseType(typeof(ApiSuccessResult<long>), StatusCodes.Status200OK)]
        public async Task<IActionResult> CreateNotification([FromForm] CreateNotificationCommand command)
        {
            var response = await _mediator.Send(command);

            return Ok(new ApiSuccessResult<long>(
                data: response,
                message: string.Format(SuccessMessage.MSG_CREATE_SUCCESS, "Thông báo")));
        }

        /// <summary>
        /// Gửi thông báo cho 1 người dùng
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost(NotificationRoutes.SendById)]
        [ProducesResponseType(typeof(ApiSuccessResult<bool>), StatusCodes.Status200OK)]
        public async Task<IActionResult> SendNotificationOneUser([FromForm] SendNotificationByCustomerIdCommand command)
        {
            var response = await _mediator.Send(command);

            return Ok(new ApiSuccessResult<bool>(
                data: response,
                message: string.Format("Gửi thông báo thành công!")));
        }

        /// <summary>
        /// Gửi thông báo cho tất cả khách hàng
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost(NotificationRoutes.SendAll)]
        [ProducesResponseType(typeof(ApiSuccessResult<bool>), StatusCodes.Status200OK)]
        public async Task<IActionResult> SendNotificationAllUser([FromBody] SendNotificationToAllCustomerCommand command)
        {
            var response = await _mediator.Send(command);

            return Ok(new ApiSuccessResult<bool>(
                data: response,
                message: string.Format("Hoàn tất gửi thông báo cho tất cả khách hàng!")));
        }
    }
}
