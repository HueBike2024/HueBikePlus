using Core.Infrastructure.Controllers;
using Core.Models;
using Core.Properties;
using MasterData.Application.Commands.BikeCommand;
using MasterData.Application.DTOs.Bike;
using MasterData.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using static MasterData.API.Commons.Routes;

namespace MasterData.API.Controllers
{
    [Route(BikeRoutes.Prefix)]
    public class BikeController : BaseApiController
    {
        private readonly IMediator _mediator;
        private readonly IBikeQuery _query;

        public BikeController(IMediator mediator, IBikeQuery query)
        {
            _mediator = mediator;
            _query = query;
        }
        /// <summary>
        ///  Tạo 1 xe
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost(BikeRoutes.Create)]
        [ProducesResponseType(typeof(ApiSuccessResult<bool>), StatusCodes.Status200OK)]
        public async Task<IActionResult> CreateBike([FromBody] CreateBikeCommand command)
        {
            // Capture the current host and scheme
            var requestHost = $"{Request.Scheme}://{Request.Host}";
            //var requestHost = _configuration.GetValue<string>("Ngrok:BaseUrl");
            command.BaseQrUrl = requestHost;

            var response = await _mediator.Send(command);

            return Ok(new ApiSuccessResult<bool>(
                data: response,
                message: string.Format(SuccessMessage.MSG_CREATE_SUCCESS, "Xe")));
        }

        /// <summary>
        /// Chỉnh sửa thông tin xe
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut(BikeRoutes.Create)]
        [ProducesResponseType(typeof(ApiSuccessResult<bool>), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateBike([FromBody] CreateBikeCommand command)
        {
            var response = await _mediator.Send(command);

            return Ok(new ApiSuccessResult<bool>(
                data: response,
                message: string.Format(SuccessMessage.MSG_UPDATE_SUCCESS, "Xe")));
        }
        /// <summary>
        /// Xóa 1 xe
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>

        [HttpDelete(BikeRoutes.Delete)]
        [ProducesResponseType(typeof(ApiSuccessResult<bool>), StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteBike(DeleteBikeCommand command)
        {
            var response = await _mediator.Send(command);

            return Ok(new ApiSuccessResult<bool>(
                data: response,
                message: string.Format(SuccessMessage.MSG_DELETE_SUCCESS, "Bike")));
        }

        /// <summary>
        ///  Kích hoạt xe
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost(BikeRoutes.Active)]
        [ProducesResponseType(typeof(ApiSuccessResult<bool>), StatusCodes.Status200OK)]
        public async Task<IActionResult> ActiveBike([FromBody] ActiveBikeCommand command)
        {
            var response = await _mediator.Send(command);

            return Ok(new ApiSuccessResult<bool>(
                data: response,
                message: string.Format("Kích hoạt thành công!")));
        }

        /// <summary>
        ///  Tắt kích hoạt xe
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost(BikeRoutes.UnActive)]
        [ProducesResponseType(typeof(ApiSuccessResult<bool>), StatusCodes.Status200OK)]
        public async Task<IActionResult> UnActiveBike([FromBody] UnActiveBikeCommand command)
        {
            var response = await _mediator.Send(command);

            return Ok(new ApiSuccessResult<bool>(
                data: response,
                message: string.Format("Tắt kích hoạt thành công!")));
        }

        /// <summary>
        /// Chi tiết thông tin 1 xe
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpGet(BikeRoutes.Detail)]
        [ProducesResponseType(typeof(ApiSuccessResult<BikeDetailResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetBike([FromQuery] GetBikeCommand command)
        {
            var response = await _query.GetAsync(command);

            return Ok(new ApiSuccessResult<BikeDetailResponse>(
                data: response));
        }


        /// <summary>
        /// Danh sách xe
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpGet(UnitRoutes.List)]
        [ProducesResponseType(typeof(ApiSuccessResult<IList<BikeResponse>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> ListBike([FromQuery] ListBikeCommand command)
        {
            var response = await _query.ListAsync(command);

            return Ok(new ApiSuccessResult<IList<BikeResponse>>
            {
                Data = response.Data,
                Paging = response.Paging,
                Message = null
            });
        }


    }
}   
