using Core.Helpers.Cryptography;
using Infrastructure.AggregatesModel.Authen.AccountAggregate;
using Infrastructure.EF;

namespace Infrastructure.Seeders
{
    public class AuthSeeder
    {
        protected AuthSeeder() { }

        public static async Task SeedAsync(BaseDbContext context)
        {
            await SeedUserAsync(context);
        }

        public static async Task  SeedUserAsync(BaseDbContext context)
        {
            if (!context.Users.Any(e => e.UserName == "admin"))
            {
                string password = SHACryptographyHelper.SHA256Encrypt("admin123");
                var admin = new User
                {
                    UserName = "admin",
                    FullName = "supper admin",
                    Password = password,
                    Email = "admin@3si.vn",
                    IsActive = true,
                    IsSuperAdmin = true,
                };
                context.Users.Add(admin);

                await context.SaveChangesAsync();
            }
        }
    }
}
