using Authen.Application.Commands;
using Authen.Application.Queries;
using Core.Infrastructure.Controllers;
using Core.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Authen.API.Controllers
{
    [Route("api/[controller]")]
    public class TestController : BaseApiController
    {
        private readonly IMediator _mediator;
        private readonly ITestQuery _query;

        public TestController(IMediator mediator, ITestQuery query)
        {
            _mediator = mediator;
            _query = query;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetUser([FromQuery] TestCommand command)
        {
            var response = await _mediator.Send(command);

            return Ok(new ApiSuccessResult<bool>(
                data: response));
        }
    }
}
