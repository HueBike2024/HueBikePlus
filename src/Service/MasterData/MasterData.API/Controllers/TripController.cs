using Core.Infrastructure.Controllers;
using Core.Models;
using MasterData.Application.Commands.TicketCommand;
using MasterData.Application.Commands.TripCommand;
using MasterData.Application.Commands.UserCommand;
using MasterData.Application.DTOs.Ticket;
using MasterData.Application.DTOs.Trip;
using MasterData.Application.DTOs.User;
using MasterData.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using static MasterData.API.Commons.Routes;

namespace MasterData.API.Controllers
{
    [Route(TripRoutes.Prefix)]
    public class TripController : BaseApiController
    {

        private readonly IMediator _mediator;
        private readonly ITripQuery _query;

        public TripController(IMediator mediator, ITripQuery query)
        {
            _mediator = mediator;
            _query = query;
        }

        /// <summary>
        /// Danh sách tất cả chuyến đi 
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpGet(TripRoutes.List)]
        [ProducesResponseType(typeof(ApiSuccessResult<IList<ListTripResponse>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> ListAllTrip([FromQuery] ListTripCommand command)
        {
            var response = await _query.ListTripAsync(command);

            return Ok(new ApiSuccessResult<IList<ListTripResponse>>
            {
                Data = response.Data,
                Paging = response.Paging,
                Message = null
            });
        }

        /// <summary>
        /// Lấy thông tin xe khi quét mã Qr
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpGet(TripRoutes.NewTrip)]
        [ProducesResponseType(typeof(ApiSuccessResult<IList<GetBikeInfoForTripResponse>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> ListUser([FromQuery] GetBikeInfoForTripCommand command)
        {
            var response = await _query.GetBikeInfoAsync(command);

            return Ok(new ApiSuccessResult<GetBikeInfoForTripResponse>(
                data: response));
        }

        /// <summary>
        /// Danh sách chuyến đi đang diễn ra
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpGet(TripRoutes.ListOnTrip)]
        [ProducesResponseType(typeof(ApiSuccessResult<IList<UserTripResponse>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> ListOnTrip([FromQuery] ListUserTripCommand command)
        {
            var response = await _query.ListOnTripUserAsync(command);

            return Ok(new ApiSuccessResult<IList<UserTripResponse>>
            {
                Data = response.Data,
                Paging = response.Paging,
                Message = null
            });
        }

        /// <summary>
        /// Danh sách lịch sử chuyến đi 
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpGet(TripRoutes.TripHistory)]
        [ProducesResponseType(typeof(ApiSuccessResult<IList<ListTripEndedResponse>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> ListEndedTrip([FromQuery] ListUserTripCommand command)
        {
            var response = await _query.ListEndedTripUserAsync(command);

            return Ok(new ApiSuccessResult<IList<ListTripEndedResponse>>
            {
                Data = response.Data,
                Paging = response.Paging,
                Message = null
            });
        }

        /// <summary>
        /// Bắt đầu chuyến đi
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost(TripRoutes.Start)]
        [ProducesResponseType(typeof(ApiSuccessResult<bool>), StatusCodes.Status200OK)]
        public async Task<IActionResult> NewTrip([FromForm] NewTripCommand command)
        {
            var response = await _mediator.Send(command);

            return Ok(new ApiSuccessResult<bool>(
                data: response,
                message: string.Format("Chuyến đi đã bắt đầu, hãy tận hưởng!")));
        }

        /// <summary>
        /// Kết thúc chuyến đi
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost(TripRoutes.EndTrip)]
        [ProducesResponseType(typeof(ApiSuccessResult<TripResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> EndTrip([FromForm] CompleteTripCommand command)
        {
            var response = await _mediator.Send(command);

            return Ok(new ApiSuccessResult<TripResponse>(
                data: response,
                message: string.Format("Kết thúc chuyến đi thành công, chuyển hướng sang đánh giá chuyến đi!")));
        }
    }
}
