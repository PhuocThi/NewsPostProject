using News.Web.Models.Domain;

namespace News.Web.Repositories
{
    public interface INewsPostLikeRepository
    {
        Task<int> GetTotalLikes(Guid newsPostId);

        Task<IEnumerable<NewsPostLike>> GetLikesForNews(Guid newsPostId);

        Task<NewsPostLike> AddLikeForNews(NewsPostLike newsPostLike);
    }
}
