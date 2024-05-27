using Core.Exceptions;
using Core.Infrastructure.Controllers;
using Core.Models;
using Core.Properties;
using Infrastructure.AggregatesModel.MasterData.PaymentConst;
using Infrastructure.AggregatesModel.MasterData.UserAggregate;
using MasterData.Application.Commands.CategoryTicketCommand;
using MasterData.Application.Commands.NotificationCommand;
using MasterData.Application.Commands.TransactionCommmand;
using MasterData.Application.DTOs.CategoryTicket;
using MasterData.Application.DTOs.Notification;
using MasterData.Application.DTOs.Transaction;
using MasterData.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using static MasterData.API.Commons.Routes;

namespace MasterData.API.Controllers
{
    [Route(TransactionRoutes.Prefix)]
    public class TransactionController : BaseApiController
    {
        private readonly IMediator _mediator;
        private readonly ITransactionQuery _query;
        private readonly IConfiguration _configuration;

        public TransactionController(IMediator mediator, ITransactionQuery query, IConfiguration configuration)
        {
            _mediator = mediator;
            _query = query;
            _configuration = configuration;
        }

        /// <summary>
        /// Danh sách tất cả giao dịch
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpGet(TransactionRoutes.ListAll)]
        [ProducesResponseType(typeof(ApiSuccessResult<IList<ListTransactionResponse>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> ListAllTransaction([FromQuery] ListTransactionCommand command)
        {
            var response = await _query.ListAllAsync(command);

            return Ok(new ApiSuccessResult<IList<ListTransactionResponse>>
            {
                Data = response.Data,
                Paging = response.Paging,
                Message = null
            });
        }

        /// <summary>
        /// Lịch sử giao dịch của người dùng
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpGet(TransactionRoutes.ListUser)]
        [ProducesResponseType(typeof(ApiSuccessResult<IList<ListTransactionUserResponse>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> ListTransactionUser([FromQuery] ListTransactionUserCommand command)
        {
            var response = await _query.ListUserTranAsync(command);

            return Ok(new ApiSuccessResult<IList<ListTransactionUserResponse>>
            {
                Data = response.Data,
                Paging = response.Paging,
                Message = null
            });
        }

        /// <summary>
        /// Thông tin chi tiết 1 giao dịch
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpGet(TransactionRoutes.Detail)]
        [ProducesResponseType(typeof(ApiSuccessResult<PaymentResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetTransactionDetail([FromQuery] TransactionDetailCommand command)
        {
            var response = await _query.GetAsync(command);

            return Ok(new ApiSuccessResult<PaymentResponse>(
                data: response));
        }

        /// <summary>
        /// Thông tin ví tiền của khách hàng
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpGet(TransactionRoutes.WalletInfo)]
        [ProducesResponseType(typeof(ApiSuccessResult<WalletInfoResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetWalletInfo([FromQuery] WalletInfoCommand command)
        {
            var response = await _query.GetWalletAsync(command);

            return Ok(new ApiSuccessResult<WalletInfoResponse>(
                data: response));
        }

        /// <summary>
        /// Gọi command để lấy URL thanh toán
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost(TransactionRoutes.GetPaymentUrl)]
        [ProducesResponseType(typeof(ApiSuccessResult<string>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetPaymentUrl([FromBody] GetPaymentUrlCommand command)
        {
            try
            {
                // Capture the current host and scheme from configuration
                //var requestHost = $"{Request.Scheme}://{Request.Host}";
                var requestHost = _configuration.GetValue<string>("Ngrok:BaseUrl");
                command.BaseUrl = requestHost;

                var paymentUrl = await _mediator.Send(command);

                return Ok(new ApiSuccessResult<string>(
                    data: paymentUrl,
                    message: "Lấy URL thanh toán thành công!"));
            }
            catch (Exception ex)
            {
                // Handle exceptions and return an error response
                return StatusCode(StatusCodes.Status500InternalServerError, new BaseException("Có lỗi xảy ra khi lấy URL thanh toán.", ex.Message));
            }
        }

        /// <summary>
        /// Lấy kết quả thanh toán
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpGet(TransactionRoutes.GetPaymentResult)]
        [ProducesResponseType(typeof(ApiSuccessResult<PaymentResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetPaymentMessage([FromQuery] GetPaymentResultCommand command)
        {
            if (command == null)
            {
                return BadRequest(new ApiErrorResult<PaymentResponse>("Không tìm thấy dũ liệu yêu cầu!"));
            }

            try
            {
                var payment = await _mediator.Send(command);

                if (payment == null)
                {
                    return BadRequest(new ApiErrorResult<PaymentResponse>("Xử lí thanh toán không thành công!"));
                }

                return Ok(new ApiSuccessResult<PaymentResponse>(
                    data: payment,
                    message: "Thanh toán thành công!"));
            }
            catch (BaseException ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiErrorResult<PaymentResponse>(ex.Message));
            }
        }


        /// <summary>
        /// Giao dịch nạp tiền
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost(TransactionRoutes.Recharge)]
        [ProducesResponseType(typeof(ApiSuccessResult<PaymentResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Recharge([FromBody] RechargeCommmand command)
        {
            var response = await _mediator.Send(command);

            return Ok(new ApiSuccessResult<PaymentResponse>(
                data: response,
                message: string.Format("Nạp tiền thành công!")));
        }
    }
}
