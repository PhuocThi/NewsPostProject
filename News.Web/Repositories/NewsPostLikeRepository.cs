
using Microsoft.EntityFrameworkCore;
using News.Web.Data;
using News.Web.Models.Domain;

namespace News.Web.Repositories
{
    public class NewsPostLikeRepository : INewsPostLikeRepository
    {
        private readonly NewsDbContext newsDbContext;

        public NewsPostLikeRepository(NewsDbContext newsDbContext)
        {
            this.newsDbContext = newsDbContext;
        }

        public async Task<NewsPostLike> AddLikeForNews(NewsPostLike newsPostLike)
        {
            await newsDbContext.NewsPostLikes.AddAsync(newsPostLike);
            await newsDbContext.SaveChangesAsync();
            return newsPostLike;
        }

        public async Task<IEnumerable<NewsPostLike>> GetLikesForNews(Guid newsPostId)
        {
            return await newsDbContext.NewsPostLikes.Where(e=>e.NewsPostId == newsPostId).ToListAsync();
        }

        public async Task<int> GetTotalLikes(Guid newsPostId)
        {
            return await newsDbContext.NewsPostLikes.CountAsync(x=>x.NewsPostId == newsPostId);    
        }
    }
}
