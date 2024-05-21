using Azure.Core;
using Core.Exceptions;
using Core.Infrastructure.Handlers;
using Core.Properties;
using Core.SeedWork;
using Core.SeedWork.Repository;
using Infrastructure.AggregatesModel.Authen.AccountAggregate;
using Infrastructure.AggregatesModel.MasterData.UserAggregate;
using MasterData.Application.Commands.TransactionCommmand;
using MasterData.Application.Commands.UnitCommand;
using MasterData.Application.DTOs.Notification;
using MasterData.Application.DTOs.Transaction;
using MasterData.Application.DTOs.Unit;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace MasterData.Application.Queries
{
    public interface ITransactionQuery
    {
        /// <summary>
        /// Chi tiết 1 giao dịch
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<PaymentResponse> GetAsync(TransactionDetailCommand command);
        /// <summary>
        /// Thông tin ví của người dùng
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<WalletInfoResponse> GetWalletAsync(WalletInfoCommand command);
        /// <summary>
        /// Danh sách giao dịch
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<PagingResultSP<ListTransactionResponse>> ListAllAsync(ListTransactionCommand command);
        /// <summary>
        /// Danh sách giao dịch của khách hàng
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<PagingResultSP<ListTransactionUserResponse>> ListUserTranAsync(ListTransactionUserCommand command);
    }

    public class TransactionQuery : BaseHandler, ITransactionQuery
    {
        private readonly IRepository<Transaction> _tranRep;
        private readonly IRepository<UserPayment> _paymentRep;
        private readonly IRepository<User> _userRep;
        public TransactionQuery(IRepository<Transaction> tranRep, IRepository<User> userRep, IRepository<UserPayment> paymentRep)
        {
            _tranRep = tranRep;
            _userRep = userRep;
            _paymentRep = paymentRep;
        }

        public async Task<PaymentResponse> GetAsync(TransactionDetailCommand command)
        {
            var transaction = await _tranRep.FindOneAsync(e => e.Id == command.TransactionId);
            var payment = await _paymentRep.FindOneAsync(e => e.VpcMerchTxnRef == transaction.TransactionCode);
            if (transaction == null)
            {
                throw new BaseException("Không tìm thấy giao dịch");
            }

            var transactionResponse = (from Transaction in _tranRep.GetQuery()
                                       join User in _userRep.GetQuery() on Transaction.UserId equals User.Id
                                       where Transaction.Id == command.TransactionId
                                       select new PaymentResponse
                                       {
                                           TradingName = transaction.TransactionType,
                                           VpcMerchTxnRef = transaction.TransactionCode,
                                           CardNum = transaction.TransactionType == "Nạp điểm" ? payment.VpcCardNum : null,
                                           TotalPrice = transaction.Point,
                                           UserFullName = User.FullName,
                                           TradingStatus = transaction.IsSuccess ? "Thành công" : "Thất bại",
                                           TradingDate = transaction.CreatedDate,
                                       }).FirstOrDefaultAsync(); // hoặc SingleOrDefaultAsync()

            return await transactionResponse;
        }

        public async Task<WalletInfoResponse> GetWalletAsync(WalletInfoCommand command)
        {
            var user = await _userRep.FindOneAsync(e => e.Id == UserId);
            if (user == null)
            {
                throw new BaseException("Không tìm thấy khách hàng");
            }

            var walletResponse = new WalletInfoResponse();

            if (user.Point < 0)
            {
                walletResponse.CurrentPoint = 0;
                walletResponse.DebtCharge = user.Point;

                return walletResponse;
            }
            walletResponse.CurrentPoint = user.Point;
            walletResponse.DebtCharge = 0;


            return walletResponse;
        }

        public async Task<PagingResultSP<ListTransactionResponse>> ListAllAsync(ListTransactionCommand request)
        {
            var transactionResponse = from Transaction in _tranRep.GetQuery()
                                               join User in _userRep.GetQuery() on Transaction.UserId equals User.Id
                                               select new ListTransactionResponse
                                               {
                                                   Id = Transaction.Id,
                                                   TransactionType = Transaction.TransactionType,
                                                   Point = Transaction.Point,
                                                   TraderName = User.FullName,
                                                   CreatedDate = Transaction.CreatedDate,
                                               };

            if (!string.IsNullOrEmpty(request.SearchTerm))
            {
                request.SearchTerm = request.SearchTerm.ToLower().Trim();

                request.SearchTerm = request.SearchTerm.ToLower().Trim();
                transactionResponse = transactionResponse.Where(e =>
                    e.TransactionType.ToLower().Contains(request.SearchTerm) ||
                    e.TraderName.ToLower().Contains(request.SearchTerm) 
                );
            }

            if (request.StartDate == null && request.EndDate != null)
            {
                transactionResponse = transactionResponse.Where(e =>
                    e.CreatedDate <= request.EndDate
                );
            }

            if (request.StartDate != null && request.EndDate == null)
            {
                transactionResponse = transactionResponse.Where(e =>
                    e.CreatedDate >= request.StartDate
                );
            }

            if (request.StartDate != null && request.EndDate != null)
            {
                transactionResponse = transactionResponse.Where(e =>
                    request.StartDate <= e.CreatedDate && e.CreatedDate <= request.EndDate
                );
            }

            if (string.IsNullOrEmpty(request.OrderBy) && string.IsNullOrEmpty(request.OrderByDesc))
            {
                transactionResponse = transactionResponse.OrderByDescending(e => e.CreatedDate);
            }
            else
            {
                transactionResponse = PagingSorting.Sorting(request, transactionResponse);
            }
            var pageIndex = request.PageSize * (request.PageIndex - 1);

            var response = await PaginatedList<ListTransactionResponse>.CreateAsync(transactionResponse, pageIndex, request.PageSize);

            var result = new PagingResultSP<ListTransactionResponse>(response, response.Total, request.PageIndex, request.PageSize);
            var i = pageIndex + 1;

            foreach (var item in result.Data)
            {
                item.Index = i++;
            }

            return result;

        }

        public async Task<PagingResultSP<ListTransactionUserResponse>> ListUserTranAsync(ListTransactionUserCommand request)
        {
            var transactionResponse = from Transaction in _tranRep.GetQuery()
                        join User in _userRep.GetQuery() on Transaction.UserId equals User.Id
                        where Transaction.UserId == UserId
                        select new ListTransactionUserResponse
                        {
                            Id = Transaction.Id,
                            TransactionType = Transaction.TransactionType,
                            Point = Transaction.Point,
                            CreatedDate = Transaction.CreatedDate,
                        };

            if (!string.IsNullOrEmpty(request.SearchTerm))
            {
                request.SearchTerm = request.SearchTerm.ToLower().Trim();

                request.SearchTerm = request.SearchTerm.ToLower().Trim();
                transactionResponse = transactionResponse.Where(e =>
                    e.TransactionType.ToLower().Contains(request.SearchTerm)
                );
            }

            if (request.StartDate == null && request.EndDate != null)
            {
                transactionResponse = transactionResponse.Where(e =>
                    e.CreatedDate <= request.EndDate
                );
            }

            if (request.StartDate != null && request.EndDate == null)
            {
                transactionResponse = transactionResponse.Where(e =>
                    e.CreatedDate >= request.StartDate
                );
            }

            if (request.StartDate != null && request.EndDate != null)
            {
                transactionResponse = transactionResponse.Where(e =>
                    request.StartDate <= e.CreatedDate && e.CreatedDate <= request.EndDate
                );
            }


            if (string.IsNullOrEmpty(request.OrderBy) && string.IsNullOrEmpty(request.OrderByDesc))
            {
                transactionResponse = transactionResponse.OrderByDescending(e => e.CreatedDate);
            }
            else
            {
                transactionResponse = PagingSorting.Sorting(request, transactionResponse);
            }
            var pageIndex = request.PageSize * (request.PageIndex - 1);

            var response = await PaginatedList<ListTransactionUserResponse>.CreateAsync(transactionResponse, pageIndex, request.PageSize);
            var result = new PagingResultSP<ListTransactionUserResponse>(response, response.Total, request.PageIndex, request.PageSize);
            var i = pageIndex + 1;

            foreach (var item in result.Data)
            {
                item.Index = i++;
            }

            return result;
        }
    }
}
