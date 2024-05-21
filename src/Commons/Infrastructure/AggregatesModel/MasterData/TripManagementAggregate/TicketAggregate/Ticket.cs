using Core.Models.Base;
using Infrastructure.AggregatesModel.Authen.AccountAggregate;
using Infrastructure.AggregatesModel.MasterData.BikeManagementAggregate.BikeAggregate;
using Infrastructure.AggregatesModel.MasterData.StatusAggregate;
using Infrastructure.AggregatesModel.MasterData.TripManagementAggregate.TripAggregate;
using MediatR.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.AggregatesModel.MasterData.TripManagementAggregate.TicketAggregate
{
    public class Ticket : BaseEntity
    {
        public string TicketNo { get; set; } = string.Empty;
        public string UserFullName { get; set; } = string.Empty;
        public string UserPhone { get; set; } = string.Empty;
        public string? PathQr {  get; set; }
        public string? QrImage {  get; set; }
        public DateTime BookingDate { get; set; }
        public DateTime? ExpectedEndTime { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public long CategoryTicketId { get; set; }
        public long UserId { get; set;}
        public long? BikeId { get; set; }
        public decimal Price { get; set; }
        public long StatusId { get; set; }

        public virtual User User { get; set; }
        public virtual Bike Bike { get; set; }
        public virtual CategoryTicket CategoryTicket { get; set; }
        public virtual Trip Trip { get; set; }
        public virtual Status Status { get; set; }

        public Ticket()
        {
            
        }

        public Ticket(string ticketNo, string userFullName, string userPhone, string pathQr, string qrImage, DateTime bookingDate, long categoryTicketId, long userId, long? bikeId, decimal price, long statusId)
        {
            TicketNo = ticketNo;
            UserFullName = userFullName;
            UserPhone = userPhone;
            PathQr = pathQr;
            QrImage = qrImage;
            BookingDate = bookingDate;
            CategoryTicketId = categoryTicketId;
            UserId = userId;
            BikeId = bikeId;
            Price = price;
            StatusId = statusId;
        }
    }
}
