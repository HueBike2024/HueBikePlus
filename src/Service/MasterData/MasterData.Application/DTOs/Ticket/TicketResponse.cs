using Core.SeedWork.ExtendEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterData.Application.DTOs.Ticket
{
    public class TicketResponse : BaseExtendEntities
    {
        public int Index { get; set; }
        public long Id { get; set; }
        public string TicketNo { get; set; }
        public long UserId { get; set; }
        public string UserFullName { get; set; }
        public string UserPhone { get; set; }
        public DateTime? BookingDate { get; set; }
        public DateTime? ExpectedEndTime { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public string? QrImg { get; set; }
        public long? BikeId { get; set; }
        public string BikeName { get; set; }
        public long CategoryTicketId { get; set; }
        public string CategoryTicketName { get; set; }
        public decimal Price { get; set; }
        public long StatusId { get; set; }
        public string Status { get; set; }
    }
}
