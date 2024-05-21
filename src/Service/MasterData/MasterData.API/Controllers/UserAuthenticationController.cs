using Core.Infrastructure.Controllers;
using Core.Models;
using Infrastructure.AggregatesModel.MasterData.UserAggregate.AuthenticationAggregate;
using MasterData.Application.Commands.UserAuthenticationCommand;
using MasterData.Application.Commands.UserCommand;
using MasterData.Application.DTOs.UserAuthentication;
using MasterData.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using static MasterData.API.Commons.Routes;

namespace MasterData.API.Controllers
{
    [Route(UserAuthenRoutes.Prefix)]
    public class UserAuthenticationController : BaseApiController
    {
        private readonly IMediator _mediator;
        private readonly IUserQuery _query;

        public UserAuthenticationController(IMediator mediator, IUserQuery query)
        {
            _mediator = mediator;
            _query = query;
        }

        [HttpPost(UserAuthenRoutes.AddInfo)]
        [ProducesResponseType(typeof(ApiSuccessResult<AddAuthenInfoResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> AddAuthenInfo([FromBody] AddAuthenInfoCommand command)
        {
            var response = await _mediator.Send(command);

            return Ok(new ApiSuccessResult<AddAuthenInfoResponse>(
                data: response,
                message: "Xác nhận thông tin thành công!"));
        }
    }
}
