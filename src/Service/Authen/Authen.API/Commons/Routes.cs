namespace Authen.API.Commons
{
    public class Routes
    {
        public static class AccountRoutes
        {
            public const string Prefix = @"authen/api/account";
            public const string SignIn = "sign-in";
            public const string RevokeToken = "revoke-token";
            public const string SignUp = "sign-up";
            public const string ChangePassword = "change-password";
            public const string ForgotPassword = "forgot-password";
            public const string LockAccount = "lock-account";
        }

    }
}
