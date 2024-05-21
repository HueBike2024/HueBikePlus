using Core.Infrastructure.Controllers;
using Core.Models;
using Core.Properties;
using MasterData.Application.Commands.BikeCommand;
using MasterData.Application.Commands.BikeLockCommand;
using MasterData.Application.Commands.StatusCommand;
using MasterData.Application.Commands.UnitCommand;
using MasterData.Application.Commands.UserCommand;
using MasterData.Application.DTOs.Bike;
using MasterData.Application.DTOs.BikeLock;
using MasterData.Application.DTOs.Status;
using MasterData.Application.DTOs.Unit;
using MasterData.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static MasterData.API.Commons.Routes;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace MasterData.API.Controllers
{
    [Route(LockRoutes.Prefix)]
    public class LockController : BaseApiController
    {
        private readonly IMediator _mediator;
        private readonly IBikeLockQuery _query;

        public LockController(IMediator mediator, IBikeLockQuery query)
        {
            _mediator = mediator;
            _query = query;
        }
        /// <summary>
        ///  tạo 1 khóa
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(ApiSuccessResult<long>), StatusCodes.Status200OK)]
        public async Task<IActionResult> CreateLock([FromForm] CreateLockCommand command)
        {
            var response = await _mediator.Send(command);

            return Ok(new ApiSuccessResult<long>(
                data: response,
                message: string.Format(SuccessMessage.MSG_CREATE_SUCCESS, "Khóa")));
        }
        /// <summary>
        /// Xóa 1 khóa
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>

        [HttpDelete(LockRoutes.Delete)]
        [ProducesResponseType(typeof(ApiSuccessResult<bool>), StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteLock([FromForm] DeleteLockCommand command)
        {
            var response = await _mediator.Send(command);

            return Ok(new ApiSuccessResult<bool>(
                data: response,
                message: string.Format(SuccessMessage.MSG_DELETE_SUCCESS, "Khóa")));
        }

        /// <summary>
        /// Chi tiết thông tin 1 khóa
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpGet(LockRoutes.Detail)]
        [ProducesResponseType(typeof(ApiSuccessResult<LockDetailResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetLock([FromQuery] GetLockCommand command)
        {
            var response = await _query.GetAsync(command);

            return Ok(new ApiSuccessResult<LockDetailResponse>(
                data: response));
        }

        /// <summary>
        /// Danh sách khóa
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpGet(LockRoutes.List)]
        [ProducesResponseType(typeof(ApiSuccessResult<IList<LockResponse>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> ListLock([FromQuery] ListBikelockCommand command)
        {
            var response = await _query.ListAsync(command);

            return Ok(new ApiSuccessResult<IList<LockResponse>>
            {
                Data = response.Data,
                Paging = response.Paging,
                Message = null
            });
        }


    }
}
