using Core.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.AggregatesModel.MasterData.EventAggregate
{
    public class Event : BaseEntity
    {

        public string Title { get; set; }
        public string Image { get; set; } = string.Empty;
        public string Description { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }
        public string Location { get; set; }
        public string Organizer { get; set; }




        public Event() { }
        public Event(string title, string image, string description, DateTime datestart,DateTime dateend, string location, string organizer)
        {
            Title = title;
            Image = image;
            Description = description;
            DateStart = datestart;
            DateEnd = dateend;
            Location = location;
            Organizer = organizer;
        }

        public static void Update(ref Event evt, string title, string image, string description, DateTime datestart, DateTime dateend, string location, string organizer)
        {
            evt.Title = title;
            evt.Image = image;
            evt.Description = description;
            evt.DateStart = datestart;
            evt.DateEnd = dateend;
            evt.Location = location;
            evt.Organizer = organizer;
        }
        public static void DeleteEvent(ref Event evt)
        {
            evt.IsDeleted = true;
        }

    }
}
