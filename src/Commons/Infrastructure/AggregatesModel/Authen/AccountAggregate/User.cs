using Core.Models.Base;
using Infrastructure.AggregatesModel.Authen.FunctionAggregate;
using Infrastructure.AggregatesModel.Authen.PermissionAggregate;
using Infrastructure.AggregatesModel.MasterData.ImageAggregate;
using Infrastructure.AggregatesModel.MasterData.NotificationAggregate;
using Infrastructure.AggregatesModel.MasterData.StatusAggregate;
using Infrastructure.AggregatesModel.MasterData.TripManagementAggregate.RateAggregate;
using Infrastructure.AggregatesModel.MasterData.TripManagementAggregate.TicketAggregate;
using Infrastructure.AggregatesModel.MasterData.UserAggregate;
using Infrastructure.AggregatesModel.MasterData.UserAggregate.AuthenticationAggregate;
using Infrastructure.AggregatesModel.MasterData.UserAggregate.ComplainAggregate;

namespace Infrastructure.AggregatesModel.Authen.AccountAggregate
{
    public class User : BaseUser
    {
        public string FullName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public decimal Point { get; set; } = 0;
        public long? AvatarId { get; set; }
        public long? PhotoId { get; set; }
        public long? StatusId { get; set; }
        public bool IsActive { get; set; }
        public bool IsLock { get; set; } = false;
        public bool IsConfirm { get; set; } = false;
        public long? AuthenId {  get; set; }

        public virtual Status Status { get; set; }
        public virtual UserAuthentication AuthenticationInfo { get; set; }


        public virtual AuthenMedia Avatar { get; set; }
        public virtual Avatar ProfilePhoto { get; set; }


        public virtual ICollection<Transaction> Transactions { get; set; }
        public virtual ICollection<UserPayment> PaymentHistorys { get; set; }
        public virtual ICollection<Complain> Complains { get; set; }
        public virtual ICollection<ComplainReply> ComplainReplys { get; set; }
        public virtual ICollection<Rate> Rates { get; set; }
        public virtual ICollection<RateReply> RateReplys { get; set; }
        public virtual ICollection<Ticket> Tickets { get; set; }
        public virtual ICollection<UserNotification> UserNotifications { get; set; }

        public virtual ICollection<UserToken> UserTokens { get; set; }
        public virtual ICollection<UserRefreshToken> UserRefreshTokens { get; set; }

        public virtual ICollection<UserFunction> UserFunctions { get; set; }
        public virtual ICollection<UserPermission> UserPermissions { get; set; }

        public User()
        {

        }

        public User(string fullName, string userName, string address, string email, string phoneNumber, string password/*, string timezone*/)
        {
            FullName = fullName;
            UserName = userName;
            Address = address;
            Email = email;
            PhoneNumber = phoneNumber;
            Password = password;
            //TimeZone = timezone;
        }

        public static void ResetPassword(ref User user, string password)
        {
            user.Password = password;
        }

        public static void LockUser(ref User user)
        {
            user.IsLock = true;
        }

        public static void UnLockUser(ref User user)
        {
            user.IsLock = false;
        }

        public static void EmailConfirm(ref User user)
        {
            user.IsConfirm = true;
        }

        public static void DeleteUser(ref User user)
        {
            user.IsDeleted = true;
        }


    }
}