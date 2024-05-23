namespace MasterData.API.Commons
{
    public class Routes
    {
        /// <summary>
        /// base route
        /// </summary>
        public static class UnitRoutes
        {
            public const string Prefix = @"master-data/api/unit";
            public const string List = "list";
        }

        /// <summary>
        /// thuộc tính
        /// </summary>
        public static class AttributeRoutes
        {
            public const string Prefix = @"master-data/api/attribute";
            public const string List = "list";
            public const string Toggle = "toggle";
        }

        /// <summary>
        /// người dùng
        /// </summary>
        public static class UserRoutes
        {
            public const string Prefix = @"master-data/api/user";
            public const string Create = "create";
            public const string Delete = "del";
            public const string List = "all";
            public const string Update = "update";
            public const string UserDetail = "detail";
            public const string LockUser = "lock";
            public const string UnLockUser = "Unlock";
            public const string ChangeAvatar = "avatar-change";
            public const string GrantPassword = "password-grant";
        }

        /// <summary>
        /// xác thực người dùng
        /// </summary>
        public static class UserAuthenRoutes
        {
            public const string Prefix = @"master-data/api/user/authen";
            public const string AddInfo = "add-authen-info";
            public const string SendInfo = "send-authen-info";
            public const string confirm = "authen-confirm-info";
        }

        /// <summary>
        /// Khiếu nại
        /// </summary>
        public static class ComplainRoutes
        {
            public const string Prefix = @"master-data/api/complain";
            public const string Create = "create";
            public const string Send = "send";
            public const string Detail = "detail";
            public const string Feedback = "feedback";
            public const string Delete = "del";
            public const string List = "list";
        }

        /// <summary>
        /// Phản hồi khiếu nại
        /// </summary>
        public static class ComplainReplyRoutes
        {
            public const string Prefix = @"master-data/api/complain/reply";
            public const string Create = "create";
            public const string Delete = "del";
            public const string UpdateContent = "change-content";
        }

        /// <summary>
        /// Thông báo
        /// </summary>
        public static class NotificationRoutes
        {
            public const string Prefix = @"master-data/api/notification";
            public const string Create = "create";
            public const string Detail = "detail";
            public const string ListReceived = "list-received";
            public const string ListSended = "list-sended";
            public const string SendById = "send";
            public const string SendAll = "sendall";
        }

        /// <summary>
        /// Trạng thái
        /// </summary>
        public static class StatusRoutes
        {
            public const string Prefix = @"master-data/api/status";
            public const string List = "list";
            public const string Create = "create";
            public const string Delete = "del";
            public const string Update = "update";
        }

        /// <summary>
        /// Vị trí
        /// </summary>
        public static class LocationRoutes
        {
            public const string Prefix = @"master-data/api/map-location";
            public const string List = "list";
            public const string Create = "create";
            public const string Delete = "del";
            public const string Update = "update";
        }

        /// <summary>
        /// Loại vé
        /// </summary>
        public static class CategoryTicketRoutes
        {
            public const string Prefix = @"master-data/api/category-ticket";
            public const string List = "list";
            public const string Create = "create";
            public const string Delete = "del";
            public const string Update = "update";
        }

        /// <summary>
        /// Giao dịch
        /// </summary>
        public static class TransactionRoutes
        {
            public const string Prefix = @"master-data/api/transaction";
            public const string Recharge = "recharge";
            public const string ListAll = "all-list";
            public const string ListUser = "user-list";
            public const string Detail = "detail";
            public const string WalletInfo = "user-wallet";
            public const string GetPaymentUrl = "get-payment-url";
            public const string GetPaymentResult = "payment-result-mobile";
        }

        public static class TicketRoutes
        {
            public const string Prefix = @"master-data/api/ticket";
            public const string preBook = "pre-booking";
            public const string ChangeInfo = "update-booking-info";
            public const string ListAll = "all-list";
            public const string ListPurchaed = "purchased-list";
            public const string Detail = "detail";
            public const string Using = "use-ticket";
            public const string Delete = "delete";
        }
        public static class LockRoutes
        {
            public const string Prefix = @"master-data/api/Lock";
            public const string Create = "create";
            public const string Delete = "del";
            public const string Detail = "detail";
            public const string List = "list";
        }
        /// <summary>
        /// xe
        /// </summary>
        public static class BikeRoutes
        {
            public const string Prefix = @"master-data/api/Bike";
            public const string Create = "create";
            public const string Delete = "del";
            public const string Detail = "detail";
            public const string Update = "update";
            public const string Active = "active";
            public const string UnActive = "unactive";
        }

        /// <summary>
        /// Trạm
        /// </summary>
        public static class StationRoutes
        {
            public const string Prefix = @"master-data/api/station";
            public const string List = "list";
            public const string ListStationtoUser = "liststationtouser";
            public const string Create = "create";
            public const string Delete = "del";
            public const string Update = "update";
            public const string Detail = "detail";

        }
        public static class AccountRoutes
        {
            public const string Prefix = @"master-data/api/account";
            public const string SignIn = "sign-in";
            public const string RevokeToken = "revoke-token";
            public const string SignUp = "sign-up";
            public const string ChangePassword = "change-password";
            public const string ForgotPassword = "forgot-password";
            public const string LockAccount = "lock-account";
        }
        public static class PostRoutes
        {
            public const string Prefix = @"master-data/api/post";
            public const string List = "list";
            public const string Create = "create";
            public const string Delete = "del";
            public const string Update = "update";
            public const string Detail = "detail";
        }

        public static class BannerRoutes
        {
            public const string Prefix = @"master-data/api/banner";
            
            public const string List = "list";
            public const string WebList = "list/web";
            public const string MobileList = "list/mobile";
            public const string Create = "create";
           
            public const string Delete = "del";
            public const string Update = "update";
            public const string Detail = "detail";
        }

        /// <summary>
        /// Chuyến đi
        /// </summary>
        public static class TripRoutes
        {
            public const string Prefix = @"master-data/api/trip";
            public const string List = "list";
            public const string NewTrip = "new-trip";
            public const string ListOnTrip = "list-ongoing";
            public const string TripHistory = "trip-history";
            public const string Start = "trip-start";
            public const string Delete = "del";
            public const string EndTrip = "trip-end";
            public const string Detail = "detail";
        }
    }
}
