using News.Web.Models.Domain;

namespace News.Web.Repositories
{
    public interface INewsPostRepository
    {
        Task<IEnumerable<NewsPost>> GetAllAsync();
        Task<NewsPost?> GetAsync(Guid id);
        Task<NewsPost> AddAsync(NewsPost newsPost);
        Task<NewsPost?> UpdateAsync(NewsPost newsPost);
        Task<NewsPost?> DeleteAsync(Guid id);
        Task<NewsPost?> GetByUrlHandleAsync(string urlHandle);
    }
}
