using Core.Common;
using Core.Exceptions;
using Core.Extensions;
using Core.Infrastructure.Handlers;
using Core.Interfaces.Database;
using Core.Interfaces.Jwt;
using Core.Properties;
using Core.SeedWork.Repository;
using GoogleApi.Entities.Interfaces;
using Infrastructure.AggregatesModel.Authen.AccountAggregate;
using Infrastructure.AggregatesModel.MasterData.UserAggregate;
using Infrastructure.Services;
using MasterData.Application.Commands.StatusCommand;
using MasterData.Application.DTOs.Status;
using MasterData.Application.DTOs.Transaction;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterData.Application.Commands.TransactionCommmand
{
    public class RechargeCommmand : IRequest<PaymentResponse>
    {
        public long Point { get; set; }
        public bool IsSuccess { get; set; }
    }

    public class RechargeCommmandHandler : BaseHandler, IRequestHandler<RechargeCommmand, PaymentResponse>
    {
        public readonly IRepository<Transaction> _tranRep;
        public readonly IRepository<User> _userRep;
        public readonly IUnitOfWork _unitOfWork;
        private readonly IRandomService _randomService;

        public RechargeCommmandHandler(IRepository<Transaction> tranRep, IRandomService randomService, IRepository<User> userRep, IUnitOfWork unitOfWork)
        {
            _tranRep = tranRep;
            _userRep = userRep;
            _unitOfWork = unitOfWork;
            _randomService = randomService;
        }
        public async Task<PaymentResponse> Handle(RechargeCommmand request, CancellationToken cancellationToken)
        {
            // Kiểm tra dữ liệu đầu vào
            if (request.Point <= 0)
            {
                throw new BaseException("Điểm nạp tiền phải lớn hơn 0.");
            }

            var user = await _userRep.FindOneAsync(e => e.Id == UserId);

            if (user == null)
            {
                throw new BaseException(ErrorsMessage.MSG_NOT_EXIST, "Người dùng");
            }

            // Thực hiện giao dịch
            var transaction = new Transaction("Nạp tiền" , _randomService.GenerateRandomString(16), request.Point, UserId, request.IsSuccess);
            _tranRep.Add(transaction);

            // Cập nhật điểm người dùng
            if (request.IsSuccess)
            {
                user.Point += request.Point;
                _userRep.Update(user);
            }

            // Lưu thay đổi
            await _unitOfWork.SaveChangesAsync();

            // Trả về kết quả
            var result = new PaymentResponse
            {
                TradingName = transaction.TransactionType,
                VpcMerchTxnRef = transaction.TransactionCode,
                TotalPrice = transaction.Point,
                UserFullName = user.FullName,
                TradingStatus = transaction.IsSuccess ? "Thành công" : "Thất bại",
                TradingDate = transaction.CreatedDate,
            };

            // Thông báo thành công hoặc thất bại
            if (request.IsSuccess)
            {
                return result;
            }
            else
            {
                throw new BaseException("Nạp tiền thất bại!");
            }
        }
    }
}
