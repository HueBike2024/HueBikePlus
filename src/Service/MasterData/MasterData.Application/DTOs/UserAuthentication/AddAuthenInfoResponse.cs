using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterData.Application.DTOs.UserAuthentication
{
    public class AddAuthenInfoResponse
    {
        public long Id { get; set; }
        public string FullName { get; set; }
        public string CardId { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public DateTime DayOfBirth { get; set; }
    }
}
