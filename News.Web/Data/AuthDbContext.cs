using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace News.Web.Data
{
    public class AuthDbContext : IdentityDbContext
    {
        public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            //seed roles (user, admin, superadmin)

            var AdminRoleId = "24f99eb2-f340-45be-b683-6ad5ff491540";
            var SuperAdminRoleId = "3334f8a6-ee0e-440e-9674-b962c2a43691";
            var UserRoleId = "7705be2e-11a6-4309-be61-9c71818057e3";

            var roles = new List<IdentityRole>
            {
                new IdentityRole
                {
                    Name= "Admin",
                    NormalizedName = "Admin",
                    Id =AdminRoleId,
                    ConcurrencyStamp = AdminRoleId
                },

                new IdentityRole
                {
                    Name= "SuperAdmin",
                    NormalizedName = "SuperAdmin",
                    Id =SuperAdminRoleId,
                    ConcurrencyStamp = SuperAdminRoleId
                },

                new IdentityRole
                {
                    Name= "User",
                    NormalizedName = "User",
                    Id = UserRoleId,
                    ConcurrencyStamp = UserRoleId
                },
            };

            builder.Entity<IdentityRole>().HasData(roles);

            //seed superadmin

            var superAdminId = "db7b0d6c-f405-4860-baa8-7ff7533f0bd9";
            var superAdminUser = new IdentityUser
            {
                UserName = "superadmin@news.com",
                Email = "superadmin@news.com",
                NormalizedEmail = "superadmin@news.com".ToUpper(),
                NormalizedUserName = "superadmin@news.com".ToUpper(),
                Id = superAdminId

            };
            superAdminUser.PasswordHash = new PasswordHasher<IdentityUser>()
                .HashPassword(superAdminUser, "thi123456");

            builder.Entity<IdentityUser>().HasData(superAdminUser);

            var superAdminRoles = new List<IdentityUserRole<string>>
            {
                new IdentityUserRole<string>()
                {
                    RoleId = AdminRoleId,
                    UserId = superAdminId
                },
                new IdentityUserRole<string>()
                {
                    RoleId = SuperAdminRoleId,
                    UserId = superAdminId
                },
                new IdentityUserRole<string>()
                {
                    RoleId = UserRoleId,
                    UserId = superAdminId
                }
            };
            builder.Entity<IdentityUserRole<string>>().HasData(superAdminRoles);


        }
    }
}
