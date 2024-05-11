using Microsoft.AspNetCore.Identity;

namespace News.Web.Repositories
{
    public interface IUserRepository
    {
        Task<IEnumerable<IdentityUser>> GetAll();
        Task<IdentityUser?> FindByEmail(string email);
        Task<IdentityUser?> FindByUserName(string userName);
    }
}
