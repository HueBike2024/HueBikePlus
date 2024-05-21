using Authen.Application.Commands.AccountCommand;
using Authen.Application.DTOs.Account;
using Core.Infrastructure.Controllers;
using Core.Models;
using Core.Properties;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static Authen.API.Commons.Routes;

namespace Authen.API.Controllers
{
    [Route(AccountRoutes.Prefix)]
    public class AccountController: BaseApiController
    {
        private readonly IMediator _mediator;

        public AccountController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Đăng nhập
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost(AccountRoutes.SignIn)]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ApiSuccessResult<SignInResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> SignIn([FromBody] SignInCommand command)
        {
            var response = await _mediator.Send(command);

            return Ok(new ApiSuccessResult<SignInResponse>(
                data: response,
                message: response.IsFail ? response.Message : SuccessMessage.MSG_SUCCESS_SIGN_IN,
                statusCode: response.StatusCode));
        }

        /// <summary>
        /// Đổi mật khẩu
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut(AccountRoutes.ChangePassword)]
        [ProducesResponseType(typeof(ApiSuccessResult<bool>), StatusCodes.Status200OK)]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordCommand command)
        {
            var response = await _mediator.Send(command);

            return Ok(new ApiSuccessResult<bool>(
                data: response,
                message: string.Format("Đổi mật khẩu thành công!")));
        }

        /// <summary>
        /// Quên mật khẩu
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut(AccountRoutes.ForgotPassword)]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ApiSuccessResult<bool>), StatusCodes.Status200OK)]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordCommand command)
        {
            var response = await _mediator.Send(command);

            return Ok(new ApiSuccessResult<bool>(
                data: response,
                message: response ? string.Format("Hãy kiểm tra lại email của bạn!") : string.Format("Gửi email thất bại. Hãy kiểm tra lại email của bạn!")));
        }

        /// <summary>
        /// Đăng xuất
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut(AccountRoutes.RevokeToken)]
        [ProducesResponseType(typeof(ApiSuccessResult<bool>), StatusCodes.Status200OK)]
        public async Task<IActionResult> RevokeToken([FromForm] RevokeTokenCommand command)
        {
            var response = await _mediator.Send(command);

            return Ok(new ApiSuccessResult<bool>(
                data: response,
                message: string.Format(SuccessMessage.MSG_SUCCESS, "Revoke token")));
        }

        /// <summary>
        /// Đăng ký
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost(AccountRoutes.SignUp)]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ApiSuccessResult<bool>), StatusCodes.Status200OK)]
        public async Task<IActionResult> SignUp([FromBody] SignUpCommand command)
        {
            var response = await _mediator.Send(command);

            return Ok(new ApiSuccessResult<bool>(
                data: response,
                message: SuccessMessage.MSG_SUCCESS_SIGN_UP));
        }
    }
}
