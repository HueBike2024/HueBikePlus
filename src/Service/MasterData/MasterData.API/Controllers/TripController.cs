using Core.Infrastructure.Controllers;
using Core.Models;
using MasterData.Application.Commands.TripCommand;
using MasterData.Application.Commands.UserCommand;
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
    }
}
