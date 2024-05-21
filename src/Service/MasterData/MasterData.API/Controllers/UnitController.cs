using Core.Infrastructure.Controllers;
using Core.Models;
using Core.Properties;
using MasterData.Application.Commands.UnitCommand;
using MasterData.Application.DTOs.Unit;
using MasterData.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using static MasterData.API.Commons.Routes;

namespace MasterData.API.Controllers
{
    [Route(UnitRoutes.Prefix)]
    public class UnitController: BaseApiController
    {
        private readonly IMediator _mediator;
        private readonly IUnitQuery _query;

        public UnitController(IMediator mediator, IUnitQuery query)
        {
            _mediator = mediator;
            _query = query;
        }

        /// <summary>
        /// Danh sách đơn vị
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpGet(UnitRoutes.List)]
        [ProducesResponseType(typeof(ApiSuccessResult<IList<UnitResponse>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> ListUnit([FromQuery] ListUnitCommand command)
        {
            var response = await _query.ListAsync(command);

            return Ok(new ApiSuccessResult<IList<UnitResponse>>
            {
                Data = response.Data,
                Paging = response.Paging,
                Message = null
            });
        }
        /// <summary>
        /// Chi tiết 1 đơn vị
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(ApiSuccessResult<UnitDetailResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetUnit([FromQuery] GetUnitCommand command)
        {
            var response = await _query.GetAsync(command);

            return Ok(new ApiSuccessResult<UnitDetailResponse>(
                data: response));
        }
        /// <summary>
        /// Tạo 1 đơn vị
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(ApiSuccessResult<long>), StatusCodes.Status200OK)]
        public async Task<IActionResult> CreateUnit([FromForm] CreateUnitCommand command)
        {
            var response = await _mediator.Send(command);

            return Ok(new ApiSuccessResult<long>(
                data: response,
                message: string.Format(SuccessMessage.MSG_CREATE_SUCCESS, "Unit")));
        }
        /// <summary>
        /// Cập nhật 1 đơn vị
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut]
        [ProducesResponseType(typeof(ApiSuccessResult<long>), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateUnit([FromForm] UpdateUnitCommand command)
        {
            var response = await _mediator.Send(command);

            return Ok(new ApiSuccessResult<long>(
                data: response,
                message: string.Format(SuccessMessage.MSG_UPDATE_SUCCESS, "Unit")));
        }
        /// <summary>
        /// Xóa 1 đơn vị
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpDelete]
        [ProducesResponseType(typeof(ApiSuccessResult<bool>), StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteUnit([FromBody] DeleteUnitCommand command)
        {
            var response = await _mediator.Send(command);

            return Ok(new ApiSuccessResult<bool>(
                data: response,
                message: string.Format(SuccessMessage.MSG_DELETE_SUCCESS, "Unit")));
        }
    }
}
