using Core.Exceptions;
using Core.Extensions;
using Core.Helpers;
using Core.Infrastructure.Handlers;
using Core.Interfaces.Database;
using Core.Properties;
using Core.SeedWork.Repository;
using Infrastructure.AggregatesModel.Authen.AccountAggregate;
using Infrastructure.AggregatesModel.MasterData.PaymentConst;
using Infrastructure.AggregatesModel.MasterData.UserAggregate;
using Infrastructure.Services;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace MasterData.Application.Commands.TransactionCommmand
{
    public class GetPaymentUrlCommand : IRequest<string>
    {
        public decimal Price { get; set; }
        public int Amount { get; set; } = 0;
        public decimal Discount { get; set; } = 0;
        public decimal TotalPrice { get; set; }
        public string? ReturnUrl { get; set; }
        public string? CardList { get; set; }
        public string? Redirect { get; set; }
        public string Lang { get; set; }
        public string? BaseUrl { get; set; }
    }

    public class GetPaymentUrlCommandHandler : BaseHandler, IRequestHandler<GetPaymentUrlCommand, string>
    {
        private readonly IRepository<UserPayment> _paymentRep;
        private readonly IRepository<User> _userRep;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<GetPaymentUrlCommandHandler> _logger;
        private readonly PaymentServiceOptions _options;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IRandomService _randomService;

        public GetPaymentUrlCommandHandler(IRepository<UserPayment> paymentRep,
                                           IRepository<User> userRep,
                                           IUnitOfWork unitOfWork,
                                           ILogger<GetPaymentUrlCommandHandler> logger,
                                           IOptions<PaymentServiceOptions> options,
                                           IHttpContextAccessor httpContextAccessor,
                                           IRandomService randomService)
        {
            _paymentRep = paymentRep;
            _userRep = userRep;
            _unitOfWork = unitOfWork;
            _logger = logger;
            _options = options.Value;
            _httpContextAccessor = httpContextAccessor;
            _randomService = randomService;
        }

        public async Task<string> Handle(GetPaymentUrlCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRep.FindOneAsync(e => e.Id == UserId);

            if (user == null)
            {
                throw new BaseException(ErrorsMessage.MSG_NOT_EXIST, "Người dùng");
            }

            try
            {
                var hashKey = _options.HashKey;
                var accessCode = _options.AccessCode;
                var merchantId = _options.MerchantId;
                var currency = _options.Currency;
                var paymentUrl = _options.PaymentUrl;

                if (string.IsNullOrWhiteSpace(hashKey) || string.IsNullOrWhiteSpace(accessCode) ||
                    string.IsNullOrWhiteSpace(merchantId) || string.IsNullOrWhiteSpace(currency) ||
                    string.IsNullOrWhiteSpace(paymentUrl))
                {
                    throw new ApplicationException("Payment service configuration is incomplete.");
                }

                var vpcAmount = request.TotalPrice * 100;

                // Get client IP address, considering the forwarded headers
                //var vpcTicketNo = _httpContextAccessor.HttpContext.GetClientIpAddress()
                //                 ?? _httpContextAccessor.HttpContext?.Request.Headers["X-Forwarded-For"].FirstOrDefault()
                //                 ?? _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString()
                //                 ?? HttpContextExtensions.GetLocalIPAddress();


                // Đảm bảo lấy địa chỉ IP chính xác
                var vpcTicketNo = _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString() ?? HttpContextExtensions.GetLocalIPAddress();
                var vpcCommand = "pay";
                var vpcLocale = request.Lang == "vi_VN" ? "vn" : "en";
                var vpcMerchTxnRef = _randomService.GenerateRandomString(16); // Generate a unique transaction reference
                var vpcOrderInfo = vpcMerchTxnRef;
                var vpcReturnUrl = $"{request.BaseUrl}/master-data/api/transaction/payment-result-mobile";
                var vpcVersion = 2;


                var parameters = new Dictionary<string, string>
                {
                    { "vpc_AccessCode", accessCode },
                    { "vpc_Amount", vpcAmount.ToString() },
                    { "vpc_Command", vpcCommand },
                    { "vpc_Currency", currency },
                    { "vpc_Locale", request.Lang == "vi_VN" ? "vn" : "en" },
                    { "vpc_MerchTxnRef", vpcMerchTxnRef },
                    { "vpc_Merchant", merchantId },
                    { "vpc_OrderInfo", vpcMerchTxnRef },
                    { "vpc_ReturnURL", vpcReturnUrl },
                    { "vpc_TicketNo", vpcTicketNo },
                    { "vpc_Version", "2" }
                };

                var message = string.Join("&", parameters.Select(p => $"{p.Key}={p.Value}"));
                var signature = HMACSHA256Helper.GetHash(message, hashKey);

                if (string.IsNullOrWhiteSpace(signature))
                {
                    throw new ApplicationException("Signature generation failed.");
                }

                // Encode returnUrl và chỉnh sửa lại message
                var encodedReturnUrl = Uri.EscapeDataString(vpcReturnUrl);
                parameters["vpc_ReturnURL"] = encodedReturnUrl;

                message = string.Join("&", parameters.Select(p => $"{p.Key}={p.Value}"));
                message += $"&vpc_SecureHash={signature}";

                var paymentUrlWithParams = $"{paymentUrl}{message}";

                var payment = new UserPayment(accessCode, merchantId, parameters["vpc_MerchTxnRef"], vpcAmount, vpcTicketNo, signature, request.Price, request.Amount, request.Discount, request.TotalPrice, UserId, request.Redirect);

                _paymentRep.Add(payment); 
                await _unitOfWork.SaveChangesAsync();

                return paymentUrlWithParams;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Có lỗi khi thực hiện giao dịch, vui lòng thử lại!");
                throw;
            }
        }
    }
}
