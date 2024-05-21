using Core.SeedWork.ExtendEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterData.Application.DTOs.User
{
    public class UserResponse : BaseExtendEntities
    {
        public int Index { get; set; }
        public long Id { get; set; }
        public string FullName { get; set; }
        public string UserName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public long? AvatarId { get; set; }
        public long? StatusId { get; set; }
        public bool IsActive { get; set; }
        public bool IsLock { get; set; }
        public bool IsConfirm { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
