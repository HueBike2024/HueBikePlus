using Core.Interfaces.Database;
using Core.SeedWork.Repository;
using Infrastructure.AggregatesModel.Authen.AccountAggregate;
using Infrastructure.AggregatesModel.MasterData.NotificationAggregate;
using Infrastructure.AggregatesModel.MasterData.TripManagementAggregate.TicketAggregate;
using Infrastructure.AggregatesModel.MasterData.TripManagementAggregate.TripAggregate;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MasterData.Application.Services.TripService
{
    public interface ITripService
    {
        public Task CheckTrips();
    }

    public class TripService : ITripService
    {
        private readonly IRepository<Trip> _tripRep;
        private readonly IRepository<CategoryTicket> _categoryTicketRep;
        private readonly IRepository<Ticket> _ticketRep;
        private readonly IRepository<User> _userRep;
        private readonly IRepository<Notification> _notiRep;
        private readonly IRepository<UserNotification> _userNotiRep;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<TripService> _logger;

        public TripService(IRepository<UserNotification> userNotiRep, IRepository<Notification> notiRep, IRepository<User> userRep, IRepository<Ticket> ticketRep, IRepository<Trip> tripRep, IRepository<CategoryTicket> categoryTicketRep, IUnitOfWork unitOfWork, ILogger<TripService> logger)
        {
            _notiRep = notiRep;
            _userNotiRep = userNotiRep;
            _userRep = userRep;
            _ticketRep = ticketRep;
            _tripRep = tripRep;
            _categoryTicketRep = categoryTicketRep;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task CheckTrips()
        {
            var now = DateTime.Now;
            var trips = _tripRep.Find(trip => !trip.IsEnd && !trip.IsSentNotificationDeft).ToList();

            foreach (var trip in trips)
            {
                var ticket = await _ticketRep.FindOneAsync(e => e.Id == trip.TicketId);
                var categoryTicket = await _categoryTicketRep.FindOneAsync(e => e.Id == ticket.CategoryTicketId);
                var user = await _userRep.FindOneAsync(e => e.Id == ticket.UserId);

                var tripDuration = (now - trip.StartDate).TotalMinutes;
                trip.MinutesTraveled = (int)tripDuration;

                if (tripDuration > categoryTicket.UserTime * 60)
                {
                    trip.IsDebt = true;

                    // Chuyến đi vượt quá thời gian sử dụng của vé
                    var newExcessMinutes = (int)(tripDuration - categoryTicket.UserTime * 60);
                    var intervalsExceeded = newExcessMinutes / 30;

                    if (intervalsExceeded > trip.ExcessMinutes / 30) // Chỉ cập nhật khi có sự thay đổi
                    {
                        var excessIntervals = intervalsExceeded - trip.ExcessMinutes / 30;
                        trip.ExcessMinutes = newExcessMinutes;

                        var penaltyPoints = excessIntervals * 5000; // Mỗi 30 phút trừ 5000 điểm
                        user.Point = Math.Max(-50000, user.Point - penaltyPoints); // Đảm bảo không âm điểm
                        trip.TripPrice += 5000;

                        //Lấy thông báo có sẵn trong csdl
                        var notificationSent = await _notiRep.FindOneAsync(e => e.Title == "Thông báo nợ cước");
                        if (notificationSent == null)
                        {
                            //tạo mới thông báo
                            var notification = new Notification("Thông báo nợ cước", "", "Chuyến đi của bạn đang trong tình trạng nợ cước, vui lòng gia hạn thêm!");
                            _notiRep.Add(notification);
                            await _unitOfWork.SaveChangesAsync();

                            var userNotification = new UserNotification(ticket.UserId, notification.Id, false);
                            _userNotiRep.Add(userNotification);
                            await _unitOfWork.SaveChangesAsync();

                            trip.IsSentNotificationDeft = true;
                        }
                        else
                        {
                            var userNotification = new UserNotification(ticket.UserId, notificationSent.Id, false);
                            _userNotiRep.Add(userNotification);
                            await _unitOfWork.SaveChangesAsync();

                            trip.IsSentNotificationDeft = true;
                        }
                    }
                }
            }
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
