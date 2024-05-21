using Core.SeedWork.ExtendEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterData.Application.DTOs.User
{
    public class UserDetailResponse : BaseExtendEntities
    {
        public string FullName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? AvatarUrl { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string? CardId { get; set; }
        public string PhoneNumber { get; set; }
        public bool IsConfirm { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; } 
    }
}
