using CloudinaryDotNet.Actions;
using Core.SeedWork;
using Core.SeedWork.Repository;
using Infrastructure.AggregatesModel.Authen.AccountAggregate;
using Infrastructure.AggregatesModel.Authen;
using Infrastructure.AggregatesModel.MasterData.NotificationAggregate;
using MasterData.Application.Commands.NotificationCommand;
using MasterData.Application.Commands.TicketCommand;
using MasterData.Application.DTOs.Notification;
using MasterData.Application.DTOs.Ticket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.AggregatesModel.MasterData.TripManagementAggregate.TicketAggregate;
using Infrastructure.AggregatesModel.MasterData.BikeManagementAggregate.BikeAggregate;
using Microsoft.EntityFrameworkCore;
using Core.Exceptions;
using Core.Properties;
using Infrastructure.AggregatesModel.MasterData.StatusAggregate;

namespace MasterData.Application.Queries
{
    public interface ITicketQuery
    {
        /// <summary>
        /// Chi tiết 1 thông báo
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<TicketResponse> GetAsync(TicketDetailCommand command);

        /// <summary>
        /// Danh sách vé đã mua
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<PagingResultSP<ListTicketResponse>> PurchasedListAsync(ListPurchasedTicketCommand request);

        /// <summary>
        /// Danh sách tất cả vé
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<PagingResultSP<ListTicketResponse>> AllTicketListAsync(ListTicketCommand request);
    }

    public class TicketQuery : ITicketQuery
    {

        private readonly IRepository<Ticket> _ticketRep;
        private readonly IRepository<Bike> _bikeRep;
        private readonly IRepository<User> _userRep;
        private readonly IRepository<Status> _statusRep;
        private readonly IRepository<CategoryTicket> _cateRep;
        private readonly IRepository<AuthenMedia> _media;
        public TicketQuery(IRepository<Status> statusRep, IRepository<Ticket> ticketRep, IRepository<User> userRep, IRepository<Bike> bikeRep, IRepository<CategoryTicket> cateRep, IRepository<AuthenMedia> media)
        {
            _statusRep = statusRep;
            _ticketRep = ticketRep;
            _userRep = userRep;
            _bikeRep = bikeRep;
            _cateRep = cateRep;
            _media = media;
        }

        public async Task<PagingResultSP<ListTicketResponse>> AllTicketListAsync(ListTicketCommand request)
        {
            var listTicketResponse = from Ticket in _ticketRep.GetQuery()
                                       join User in _userRep.GetQuery() on Ticket.UserId equals User.Id
                                       join Bike in _bikeRep.GetQuery() on Ticket.BikeId equals Bike.Id
                                       join CategoryTicket in _cateRep.GetQuery() on Ticket.CategoryTicketId equals CategoryTicket.Id
                                       join Status in _statusRep.GetQuery() on Ticket.StatusId equals Status.Id
                                       select new ListTicketResponse
                                       {
                                           Id = Ticket.Id,
                                           TicketNo = Ticket.TicketNo,
                                           UserId = User.Id,
                                           UserFullName = User.FullName,
                                           UserPhone = Ticket.UserPhone,
                                           BikeId = Bike.Id,
                                           BikeCode = Bike.BikeCode,
                                           CategoryTicketId = CategoryTicket.Id,
                                           CategoryTicketName = CategoryTicket.CategoryTicketName,
                                           PathQr = Ticket.PathQr,
                                           QrImage = Ticket.QrImage,
                                           BookingDate = Ticket.BookingDate,
                                           ExpectedEndTime = Ticket.ExpectedEndTime,
                                           ExpiryDate = Ticket.ExpiryDate,
                                           Price = Ticket.Price,
                                           StatusId = Status.Id,
                                           Status = Status.StatusName,
                                       };

            if (!string.IsNullOrEmpty(request.SearchTerm))
            {
                request.SearchTerm = request.SearchTerm.ToLower().Trim();
                // Thử chuyển đổi SearchTerm sang long
                long searchTermAsLong;
                bool isNumeric = long.TryParse(request.SearchTerm, out searchTermAsLong);

                listTicketResponse = listTicketResponse.Where(e =>
                    e.TicketNo.ToLower().Contains(request.SearchTerm) ||
                    e.UserFullName.ToLower().Contains(request.SearchTerm) ||
                    e.UserPhone.ToLower().Contains(request.SearchTerm) ||
                    e.BikeCode.ToLower().Contains(request.SearchTerm) ||
                    e.CategoryTicketName.ToLower().Contains(request.SearchTerm) ||
                    e.Id == searchTermAsLong || // So sánh với ID dạng long
                    (isNumeric && e.Id == searchTermAsLong) // Kiểm tra nếu SearchTerm có thể chuyển thành long
                );
            }

            if(request.BookingDate != null)
            {
                listTicketResponse = listTicketResponse.Where(e =>
                    e.BookingDate.Date == request.BookingDate.Value.Date
                );
            }

            if (request.StatusId != null)
            {
                var status = await _statusRep.FindOneAsync(e => e.Id == request.StatusId);
                listTicketResponse = listTicketResponse.Where(e =>
                    e.Status.Trim().ToLower().Contains(status.StatusName.Trim().ToLower())
                ) ;
            }


            if (string.IsNullOrEmpty(request.OrderBy) && string.IsNullOrEmpty(request.OrderByDesc))
            {
                listTicketResponse = listTicketResponse.OrderByDescending(e => e.BookingDate);
            }
            else
            {
                listTicketResponse = PagingSorting.Sorting(request, listTicketResponse);
            }
            var pageIndex = request.PageSize * (request.PageIndex - 1);

            var response = await PaginatedList<ListTicketResponse>.CreateAsync(listTicketResponse, pageIndex, request.PageSize);

            var result = new PagingResultSP<ListTicketResponse>(response, response.Total, request.PageIndex, request.PageSize);
            var i = pageIndex + 1;

            foreach (var item in result.Data)
            {
                item.Index = i++;
            }

            return result;
        }

        public async Task<TicketResponse> GetAsync(TicketDetailCommand command)
        {
            var ticket = await _ticketRep.FindOneAsync(e => e.Id == command.TicketId);
            if (ticket == null)
            {
                throw new BaseException(ErrorsMessage.MSG_NOT_EXIST, "Vé");
            }

            var status = await _statusRep.FindOneAsync(e => e.Id == ticket.StatusId);
            var bike = await _bikeRep.FindOneAsync(e => e.Id == ticket.BikeId);
            var user = await _userRep.FindOneAsync(e => e.Id == ticket.UserId);
            var categoryTicket = await _cateRep.FindOneAsync(e => e.Id == ticket.CategoryTicketId);

            // Lấy thông tin về complaint
            return await _ticketRep.GetQuery(e => e.Id == command.TicketId)
                .Select(k => new TicketResponse
                {
                    Id = k.Id,
                    TicketNo = k.TicketNo,
                    UserId = k.UserId,
                    UserFullName = k.UserFullName,
                    UserPhone = k.UserPhone,
                    BookingDate = k.BookingDate,
                    ExpectedEndTime = k.ExpectedEndTime,
                    ExpiryDate = k.ExpiryDate,
                    QrImg = k.QrImage,
                    BikeId = k.BikeId,
                    BikeCode = bike.BikeCode,
                    CategoryTicketId = categoryTicket.Id,
                    CategoryTicketName = categoryTicket.CategoryTicketName,
                    Price = k.Price,
                    StatusId = status.Id,
                    Status = status.StatusName,
                }).FirstOrDefaultAsync();
        }

        public async Task<PagingResultSP<ListTicketResponse>> PurchasedListAsync(ListPurchasedTicketCommand request)
        {
            var ticket = await _ticketRep.FindOneAsync(e => e.UserId == request.UserId);
            if (ticket == null)
            {
                throw new BaseException("Không tìm thấy vé!");
            }

            var purchsedListResponse = from Ticket in _ticketRep.GetQuery()
                                             join User in _userRep.GetQuery() on Ticket.UserId equals User.Id
                                             join Bike in _bikeRep.GetQuery() on Ticket.BikeId equals Bike.Id
                                             join CategoryTicket in _cateRep.GetQuery() on Ticket.CategoryTicketId equals CategoryTicket.Id
                                             join Status in _statusRep.GetQuery() on Ticket.StatusId equals Status.Id
                                             where Ticket.UserId == request.UserId
                                             select new ListTicketResponse
                                             {
                                                 Id = Ticket.Id,
                                                 TicketNo = Ticket.TicketNo,
                                                 UserId = request.UserId,
                                                 UserFullName = User.FullName,
                                                 UserPhone = Ticket.UserPhone,
                                                 BikeId = Bike.Id,
                                                 BikeCode = Bike.BikeCode,
                                                 CategoryTicketId = CategoryTicket.Id,
                                                 CategoryTicketName = CategoryTicket.CategoryTicketName,
                                                 PathQr = Ticket.PathQr,
                                                 QrImage = Ticket.QrImage,
                                                 BookingDate = Ticket.BookingDate,
                                                 ExpectedEndTime = Ticket.ExpectedEndTime,
                                                 ExpiryDate = Ticket.ExpiryDate,
                                                 Price = Ticket.Price,
                                                 StatusId = Ticket.StatusId,
                                                 Status = Status.StatusName,
                                             };

            if (!string.IsNullOrEmpty(request.SearchTerm))
            {
                request.SearchTerm = request.SearchTerm.ToLower().Trim();

                purchsedListResponse = purchsedListResponse.Where(e =>
                    e.TicketNo.ToLower().Contains(request.SearchTerm)
                );
            }

            if (request.StatusId != null)
            {
                var status = await _statusRep.FindOneAsync(e => e.Id == request.StatusId);
                purchsedListResponse = purchsedListResponse.Where(e =>
                    e.Status.Trim().ToLower().Contains(status.StatusName.Trim().ToLower())
                );
            }

            if (string.IsNullOrEmpty(request.OrderBy) && string.IsNullOrEmpty(request.OrderByDesc))
            {
                purchsedListResponse = purchsedListResponse.OrderByDescending(e => e.BookingDate);
            }
            else
            {
                purchsedListResponse = PagingSorting.Sorting(request, purchsedListResponse);
            }
            var pageIndex = request.PageSize * (request.PageIndex - 1);

            var response = await PaginatedList<ListTicketResponse>.CreateAsync(purchsedListResponse, pageIndex, request.PageSize);

            var result = new PagingResultSP<ListTicketResponse>(response, response.Total, request.PageIndex, request.PageSize);
            var i = pageIndex + 1;

            foreach (var item in result.Data)
            {
                item.Index = i++;
            }

            return result;
        }
    }
}
