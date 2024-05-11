using Microsoft.EntityFrameworkCore;
using News.Web.Data;
using News.Web.Models.Domain;

namespace News.Web.Repositories
{
    public class NewsPostCommentRepository : INewsPostCommentRepository
    {
        private readonly NewsDbContext newsDbContext;

        public NewsPostCommentRepository(NewsDbContext newsDbContext)
        {
            this.newsDbContext = newsDbContext;
        }
        public async Task<NewsPostComment> AddAsync(NewsPostComment postComment)
        {
            await newsDbContext.NewsPostComments.AddAsync(postComment);
            await newsDbContext.SaveChangesAsync();
            return postComment;
        }

        public async Task<IEnumerable<NewsPostComment>> GetCommentsByNewsIdAsync(Guid id)
        {
            return await newsDbContext.NewsPostComments.Where(x => x.NewsPostId == id)
                .OrderByDescending(x => x.DateAdded)
                .ToListAsync();
        }
    }
}
