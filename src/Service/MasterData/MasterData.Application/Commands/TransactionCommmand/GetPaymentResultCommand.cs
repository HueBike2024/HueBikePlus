using Core.Exceptions;
using Core.Helpers;
using Core.Infrastructure.Handlers;
using Core.Interfaces.Database;
using Core.SeedWork.Repository;
using Infrastructure.AggregatesModel.Authen.AccountAggregate;
using Infrastructure.AggregatesModel.MasterData.PaymentConst;
using Infrastructure.AggregatesModel.MasterData.UserAggregate;
using MasterData.Application.DTOs.Transaction;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MasterData.Application.Commands.TransactionCommmand
{
    public class GetPaymentResultCommand : IRequest<PaymentResponse>
    {
        public string vpc_MerchTxnRef { get; set; }
        public string vpc_TxnResponseCode { get; set; }
        public string vpc_Card { get; set; }
        public string vpc_CardNum { get; set; }
        public string vpc_OrderInfo { get; set; }
        public string vpc_TransactionNo { get; set; }
        public string vpc_SecureHash { get; set; }
    }

    public class GetPaymentResultCommandHandler : BaseHandler, IRequestHandler<GetPaymentResultCommand, PaymentResponse>
    {
        private readonly IRepository<PaymentMessage> _paymentMessageRep;
        private readonly IRepository<UserPayment> _paymentRep;
        public readonly IRepository<Transaction> _tranRep;
        private readonly IRepository<User> _userRep;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<GetPaymentResultCommandHandler> _logger;
        private readonly PaymentServiceOptions _options;

        public GetPaymentResultCommandHandler(
            IRepository<PaymentMessage> paymentMessageRep,
            IRepository<UserPayment> paymentRep,
            IUnitOfWork unitOfWork,
            IOptions<PaymentServiceOptions> options,
            ILogger<GetPaymentResultCommandHandler> logger,
            IRepository<User> userRep,
            IRepository<Transaction> tranRep)
        {
            _paymentMessageRep = paymentMessageRep;
            _paymentRep = paymentRep;
            _unitOfWork = unitOfWork;
            _logger = logger;
            _options = options.Value;
            _userRep = userRep;
            _tranRep = tranRep;
        }

        public async Task<PaymentResponse> Handle(GetPaymentResultCommand request, CancellationToken cancellationToken)
        {
            try
            {
                //var resultSecureHash = VerifySecureHash(request, _options.HashKey);
                //// Kiểm tra tính toàn vẹn
                //if (!resultSecureHash)
                //{
                //    throw new BaseException("Invalid SecureHash received.");
                //}

                // Fetch the payment message
                var message = await _paymentMessageRep.FindOneAsync(e => e.Name == request.vpc_TxnResponseCode);

                if (message != null)
                {
                    // Fetch the payment record
                    var payment = await _paymentRep.FindOneAsync(e => e.VpcMerchTxnRef == request.vpc_MerchTxnRef);
                    var user = await _userRep.FindOneAsync(e => e.Id == payment.UserId);
                    if (payment != null)
                    {
                        // Update payment record if it's not already updated
                        if (string.IsNullOrWhiteSpace(payment.VpcResponseCode))
                        {
                            payment.VpcResponseCode = message.Name;
                            payment.MessageId = message.Id;
                            payment.VpcCard = request.vpc_Card;
                            payment.VpcCardNum = request.vpc_CardNum;
                            payment.VpcOrderInfo = request.vpc_OrderInfo;
                            payment.VpcTransactionNo = request.vpc_TransactionNo;

                            _paymentRep.Update(payment);
                            await _unitOfWork.SaveChangesAsync();

                            bool isPaid = message.Name == "0";
                            if (isPaid)
                            {
                                
                                user.Point += payment.TotalPrice;
                                _userRep.Update(user);
                                //Lưu thông tin giao dịch
                                var transaction = new Transaction("Nạp điểm", payment.VpcMerchTxnRef, payment.TotalPrice, payment.UserId, true);
                                _tranRep.Add(transaction);
                                await _unitOfWork.SaveChangesAsync();
                            }
                            else
                            {
                                //Lưu thông tin giao dịch
                                var transaction = new Transaction("Nạp điểm", payment.VpcMerchTxnRef, payment.TotalPrice, payment.UserId, false);
                                _tranRep.Add(transaction);
                                await _unitOfWork.SaveChangesAsync();
                            }
                            

                            // Return PaymentResponse DTO
                            return new PaymentResponse
                            {
                                TradingName = "Nạp điểm",
                                VpcMerchTxnRef = payment.VpcMerchTxnRef,
                                CardNum = payment.VpcCardNum,
                                TotalPrice = payment.TotalPrice,
                                UserFullName = user.FullName,
                                TradingStatus = message.Description,
                                TradingDate = payment.CreatedDate,
                            };
                        }
                    }
                }

                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetPaymentResultCommandHandler: {Message}", ex.Message);
                throw new BaseException("Error processing payment message", ex.Message);
            }
        }

        private bool VerifySecureHash(GetPaymentResultCommand request, string hashKey)
        {
            var parameters = new Dictionary<string, string>
            {
                { "vpc_Card", request.vpc_Card },
                { "vpc_CardNum", request.vpc_CardNum },
                { "vpc_OrderInfo", request.vpc_OrderInfo },
                { "vpc_TransactionNo", request.vpc_TransactionNo },
                { "vpc_TxnResponseCode", request.vpc_TxnResponseCode },
                { "vpc_MerchTxnRef", request.vpc_MerchTxnRef }
            };

            // Compute the signature using the provided parameters and hash key
            var computedHash = HMACSHA256Helper.GetHash(string.Join("&", parameters.OrderBy(p => p.Key).Select(p => $"{p.Key}={p.Value}")), hashKey);

            // Compare the computed hash with the provided vpc_SecureHash
            return computedHash.Equals(request.vpc_SecureHash, StringComparison.OrdinalIgnoreCase);
        }

    }
}
