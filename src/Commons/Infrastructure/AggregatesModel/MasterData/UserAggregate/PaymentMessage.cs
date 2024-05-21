using Core.Models.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.AggregatesModel.MasterData.UserAggregate
{
    public class PaymentMessage : BaseEntity
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public PaymentMessage()
        {
            
        }
        public PaymentMessage(string name, string description)
        {
            Name = name;
            Description = description;
        }
    }
}
