using Core.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.AggregatesModel.MasterData.TripManagementAggregate.TicketAggregate
{
    public class CategoryTicket : BaseEntity
    {
        public string CategoryTicketName { get; set; }
        public string? Description { get; set; }
        public double UserTime { get; set; } // Thời gian sử dụng của vé, nếu là vé tuần hoặc tháng, đây là thời gian mỗi ngày có thể sử dụng
        public decimal Price { get; set; }

        //Lấy danh sách vé có loại vé x
        public virtual ICollection<Ticket> Tickets { get; set; }

        public CategoryTicket()
        {
            
        }

        public CategoryTicket(string categoryTicketName, string description, decimal price, double userTime)
        {
            CategoryTicketName = categoryTicketName;
            Description = description;
            Price = price;
            UserTime = userTime;
        }

        //change ticket price
        public void ChangePrice(long price)
        {
            Price = price;
        }

        //remove ticket
        public void RemoveTicket(Ticket removeTicket)
        {
            Tickets.Remove(removeTicket);
        }
    }
}
