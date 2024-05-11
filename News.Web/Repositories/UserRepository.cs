using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using News.Web.Data;

namespace News.Web.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AuthDbContext authDbContext;

        public UserRepository(AuthDbContext authDbContext)
        {
            this.authDbContext = authDbContext;
        }

        public async Task<IdentityUser?> FindByEmail(string email)
        {
            return await authDbContext.Users.FirstOrDefaultAsync(x=>x.Email == email);
            
        }

        public async Task<IdentityUser?> FindByUserName(string userName)
        {
            return await authDbContext.Users.FirstOrDefaultAsync(x=>x.UserName == userName);    
        }

        public async Task<IEnumerable<IdentityUser>> GetAll()
        {
            var users = await authDbContext.Users.ToListAsync();

            var superAdminUser = await authDbContext.Users
                        .FirstOrDefaultAsync(x=>x.Email == "superadmin@news.com");

            if(superAdminUser != null)
            {
                users.Remove(superAdminUser);
            }

            return users;
        }
    }
}
