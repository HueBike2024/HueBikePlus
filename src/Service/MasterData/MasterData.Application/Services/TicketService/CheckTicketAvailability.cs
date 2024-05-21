using CloudinaryDotNet;
using Core.Exceptions;
using Core.Interfaces.Database;
using Core.SeedWork.Repository;
using Infrastructure.AggregatesModel.MasterData.NotificationAggregate;
using Infrastructure.AggregatesModel.MasterData.StatusAggregate;
using Infrastructure.AggregatesModel.MasterData.TripManagementAggregate.TicketAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterData.Application.Services.TicketService
{
    public class CheckTicketAvailability : BackgroundService
    {
        private readonly ILogger<CheckTicketAvailability> _logger;
        private readonly IRepository<Ticket> _ticketRep;
        private readonly IRepository<Status> _statusRep;
        private readonly IRepository<Notification> _notiRep;
        private readonly IRepository<UserNotification> _uNotiRep;
        private readonly IUnitOfWork _unitOfWork;
        private readonly HashSet<long> _processedTicketIds = new HashSet<long>(); // Sử dụng HashSet để nhanh chóng kiểm tra trạng thái thông báo
        private readonly List<long> _processedTicketBefore15MIds = new List<long>(); // Danh sách tạm thời để lưu trữ ID của các vé đã gửi thông báo

        public CheckTicketAvailability(
            ILogger<CheckTicketAvailability> logger,
            IRepository<Ticket> ticketRep,
            IRepository<Status> statusRep,
            IUnitOfWork unitOfWork,
            IRepository<Notification> notiRep,
            IRepository<UserNotification> uNotiRep)
        {
            _logger = logger;
            _ticketRep = ticketRep;
            _unitOfWork = unitOfWork;
            _notiRep = notiRep;
            _uNotiRep = uNotiRep;
            _statusRep = statusRep;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (true)
            {
                if (stoppingToken.IsCancellationRequested)
                {
                    // Nếu có yêu cầu hủy bỏ, thoát khỏi vòng lặp
                    break;
                }

                try
                {
                    var status = await _statusRep.FindOneAsync(e => e.StatusName.Trim().ToLower().Contains("Chưa sử dụng".Trim().ToLower()));
                    if (status == null)
                    {
                        throw new BaseException("Không tìm thấy trạng thái!");
                    }
                    var currentTime = DateTime.Now;
                    //Truy vấn lấy về danh sách người dùng cần kiểm tra
                    var ticketsAvailability = await _ticketRep.GetQuery()
                                                .Where(e => !_processedTicketIds.Contains(e.Id) && e.BookingDate.Date == currentTime.Date && e.BookingDate.Hour == currentTime.Hour && e.BookingDate.Minute == currentTime.Minute && e.Status.StatusName.Trim().ToLower().Contains("Chưa sử dụng".Trim().ToLower()))
                                                .ToListAsync();

                    var ticketsBefore15Minutes = await _ticketRep.GetQuery()
                                                .Where(e => !_processedTicketBefore15MIds.Contains(e.Id) && currentTime >= e.BookingDate.AddMinutes(-15) && currentTime <= e.BookingDate && e.Status.StatusName.Trim().ToLower().Contains("Chưa sử dụng".Trim().ToLower()))
                                                .ToListAsync();

                    //Lấy thông báo có sẵn trong csdl
                    var notificationSentBefore15M = await _notiRep.FindOneAsync(e => e.Title == "Thông báo" && e.Content == "Vé của bạn có thể sử dụng trong 15 phút nữa!");
                    var notificationSent = await _notiRep.FindOneAsync(e => e.Title == "Thông báo" && e.Content == "Một vé của bạn hiện tại có thể sử dụng. Hãy mau sử dụng!");

                    //Kiểm tra và gửi thông báo cho vé trước 15 phút
                    if (notificationSentBefore15M == null)
                    {
                        foreach (var ticket in ticketsBefore15Minutes)
                        {
                            var notification = new Notification("Thông báo", "", "Vé của bạn có thể sử dụng trong 15 phút nữa!");

                            _notiRep.Add(notification);
                            await _unitOfWork.SaveChangesAsync();

                            var userNotification = new UserNotification(ticket.UserId, notification.Id, false);
                            _uNotiRep.Add(userNotification);
                            await _unitOfWork.SaveChangesAsync();

                            _processedTicketBefore15MIds.Add(ticket.Id);
                        }
                    }
                    else
                    {
                        foreach (var ticket in ticketsBefore15Minutes)
                        {
                            var userNotification = new UserNotification(ticket.UserId, notificationSentBefore15M.Id, false);
                            _uNotiRep.Add(userNotification);
                            await _unitOfWork.SaveChangesAsync();

                            _processedTicketBefore15MIds.Add(ticket.Id);
                        }
                    }

                    //Kiểm tra và gửi thông báo cho vé đến thời gian sử dụng
                    if (notificationSent == null)
                    {
                        foreach (var ticket in ticketsAvailability)
                        {
                            var notification = new Notification("Thông báo", "", "Một vé của bạn hiện tại có thể sử dụng. Hãy mau sử dụng!");

                            _notiRep.Add(notification);
                            await _unitOfWork.SaveChangesAsync();

                            var userNotification = new UserNotification(ticket.UserId, notification.Id, false);
                            _uNotiRep.Add(userNotification);
                            await _unitOfWork.SaveChangesAsync();

                            _processedTicketIds.Add(ticket.Id);
                        }
                    }
                    else
                    {
                        foreach (var ticket in ticketsAvailability)
                        {
                            var userNotification = new UserNotification(ticket.UserId, notificationSent.Id, false);
                            _uNotiRep.Add(userNotification);
                            await _unitOfWork.SaveChangesAsync();

                            _processedTicketIds.Add(ticket.Id);
                        }
                    }



                    await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken); // Chờ 1 phút trước khi kiểm tra lại
                }
                catch (Exception ex)
                {
                    // Ghi log và tiếp tục vòng lặp
                    _logger.LogError(ex, "Đã xảy ra lỗi khi thực hiện kiểm tra thời gian vé!");
                }
            }
            _processedTicketIds.Clear();
            _processedTicketBefore15MIds.Clear();
        }
    }
}
