using Core.Infrastructure.Controllers;
using Core.Models;
using Core.Properties;
using MasterData.Application.Commands.AccountCommand;
using MasterData.Application.Commands.UnitCommand;
using MasterData.Application.Commands.UserCommand;
using MasterData.Application.DTOs.Unit;
using MasterData.Application.DTOs.User;
using MasterData.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using static MasterData.API.Commons.Routes;

namespace MasterData.API.Controllers
{
    [Route(UserRoutes.Prefix)]
    public class UserController : BaseApiController
    {

        private readonly IMediator _mediator;
        private readonly IUserQuery _query;

        public UserController(IMediator mediator, IUserQuery query)
        {
            _mediator = mediator;
            _query = query;
        }

        /// <summary>
        /// Danh sách user
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpGet(UserRoutes.List)]
        [ProducesResponseType(typeof(ApiSuccessResult<IList<UserResponse>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> ListUser([FromQuery] ListUserCommand command)
        {
            var response = await _query.ListAsync(command);

            return Ok(new ApiSuccessResult<IList<UserResponse>>
            {
                Data = response.Data,
                Paging = response.Paging,
                Message = null
            });
        }

        /// <summary>
        /// Thông tin chi tiết 1 user
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(ApiSuccessResult<UserDetailResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetUserDetail([FromQuery] UserDetailCommand command)
        {
            var response = await _query.GetAsync(command);

            return Ok(new ApiSuccessResult<UserDetailResponse>(
                data: response));
        }

        /// <summary>
        /// Tạo mới 1 user
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost(UserRoutes.Create)]
        [ProducesResponseType(typeof(ApiSuccessResult<bool>), StatusCodes.Status200OK)]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserCommand command)
        {
            var response = await _mediator.Send(command);

            return Ok(new ApiSuccessResult<bool>(
                data: response,
                message: string.Format(SuccessMessage.MSG_CREATE_SUCCESS, "Người dùng")));
        }

        /// <summary>
        /// Chỉnh sửa thông tin user
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut(UserRoutes.Create)]
        [ProducesResponseType(typeof(ApiSuccessResult<bool>), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateUser([FromBody] CreateUserCommand command)
        {
            var response = await _mediator.Send(command);

            return Ok(new ApiSuccessResult<bool>(
                data: response,
                message: string.Format(SuccessMessage.MSG_UPDATE_SUCCESS, "Người dùng")));
        }

        /// <summary>
        /// Khóa User
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost(UserRoutes.LockUser)]
        [ProducesResponseType(typeof(ApiSuccessResult<bool>), StatusCodes.Status200OK)]
        public async Task<IActionResult> LockUser([FromBody] LockUserCommand command)
        {
            var response = await _mediator.Send(command);

            return Ok(new ApiSuccessResult<bool>(
                data: response,
                message: string.Format("Khóa người dùng thành công")));
        }

        /// <summary>
        /// Mở khóa user
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost(UserRoutes.UnLockUser)]
        [ProducesResponseType(typeof(ApiSuccessResult<bool>), StatusCodes.Status200OK)]
        public async Task<IActionResult> UnLockUser([FromBody] UnLockUserCommand command)
        {
            var response = await _mediator.Send(command);

            return Ok(new ApiSuccessResult<bool>(
                data: response,
                message: string.Format("Mở Khóa người dùng thành công")));
        }

        /// <summary>
        /// Xóa user
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpDelete(UserRoutes.Delete)]
        [ProducesResponseType(typeof(ApiSuccessResult<bool>), StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteUser(DeleteUserCommand command)
        {
            var response = await _mediator.Send(command);

            return Ok(new ApiSuccessResult<bool>(
                data: response,
                message: string.Format(SuccessMessage.MSG_DELETE_SUCCESS, "User")));
        }

        /// <summary>
        /// Thay đổi ảnh đại diện
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut(UserRoutes.ChangeAvatar)]
        [ProducesResponseType(typeof(ApiSuccessResult<bool>), StatusCodes.Status200OK)]
        public async Task<IActionResult> ChangeUserAvatar([FromForm] ChangeAvatarCommand command)
        {
            var response = await _mediator.Send(command);

            return Ok(new ApiSuccessResult<bool>(
                data: response,
                message: string.Format("Thay đổi ảnh thành công!")));
        }

        /// <summary>
        /// Cấp lại mật khẩu
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut(UserRoutes.GrantPassword)]
        [ProducesResponseType(typeof(ApiSuccessResult<bool>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GrantPassword([FromForm] GrantPasswordCommand command)
        {
            var response = await _mediator.Send(command);

            return Ok(new ApiSuccessResult<bool>(
                data: response,
                message: string.Format("Cấp lại mật khẩu thành công, mật khẩu đã được gửi đến email người dùng")));
        }
    }
}
