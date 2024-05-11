using News.Web.Models.Domain;

namespace News.Web.Repositories
{
    public interface INewsPostCommentRepository
    {
        Task<NewsPostComment> AddAsync(NewsPostComment postComment);

        Task<IEnumerable<NewsPostComment>> GetCommentsByNewsIdAsync(Guid id);
    }
}
