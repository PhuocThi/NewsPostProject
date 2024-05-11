using Microsoft.EntityFrameworkCore;
using News.Web.Models.Domain;

namespace News.Web.Data
{
    public class NewsDbContext : DbContext
    {
        public NewsDbContext(DbContextOptions<NewsDbContext> options) : base(options)
        {
        }

        public DbSet<NewsPost> NewsPosts { get; set; }
        public DbSet<Tag> Tags { get; set; }    
        public DbSet<NewsPostLike> NewsPostLikes { get; set; }
        public DbSet<NewsPostComment> NewsPostComments { get; set; }
    }
}
