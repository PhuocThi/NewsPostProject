using Microsoft.EntityFrameworkCore;
using News.Web.Data;
using News.Web.Models.Domain;

namespace News.Web.Repositories
{
    public class NewsPostRepository : INewsPostRepository
    {
        private readonly NewsDbContext newsDbContext;

        public NewsPostRepository(NewsDbContext newsDbContext)
        {
            this.newsDbContext = newsDbContext;
        }
        public async Task<NewsPost> AddAsync(NewsPost newsPost)
        {
            await newsDbContext.NewsPosts.AddAsync(newsPost);
            await newsDbContext.SaveChangesAsync();
            return newsPost;
        }

        public async Task<NewsPost?> DeleteAsync(Guid id)
        {
            var item = await newsDbContext.NewsPosts.FirstOrDefaultAsync(x => x.Id == id);
            if(item != null)
            {
                newsDbContext.NewsPosts.Remove(item);
                await newsDbContext.SaveChangesAsync();
                return item;
            }
            return null;
        }

        public async Task<IEnumerable<NewsPost>> GetAllAsync()
        {
            return await newsDbContext.NewsPosts.Include(x=>x.Tags).ToListAsync();
        }

        public async Task<NewsPost?> GetAsync(Guid id)
        {
            return await newsDbContext.NewsPosts.Include(x=>x.Tags).FirstOrDefaultAsync(x=>x.Id == id);
        }

        public async Task<NewsPost?> GetByUrlHandleAsync(string urlHandle)
        {
            return await newsDbContext.NewsPosts.Include(e=>e.Tags)
                                      .FirstOrDefaultAsync(e=>e.UrlHandle == urlHandle);
        }

        public async Task<NewsPost?> UpdateAsync(NewsPost newsPost)
        {
            var item = await newsDbContext.NewsPosts.Include(e=>e.Tags)
                .FirstOrDefaultAsync(e => e.Id == newsPost.Id);

            if(item != null)
            {
                item.Id = newsPost.Id;
                item.Heading = newsPost.Heading;
                item.PageTitle = newsPost.PageTitle;
                item.Content = newsPost.Content;
                item.ShortDescription = newsPost.ShortDescription;
                item.Author = newsPost.Author;
                item.FeaturedImageUrl = newsPost.FeaturedImageUrl;
                item.UrlHandle = newsPost.UrlHandle;
                item.PublishedDate = newsPost.PublishedDate;
                item.Visible = newsPost.Visible; 
                item.Tags = newsPost.Tags;

                await newsDbContext.SaveChangesAsync();
                return item;
            }
            return null;
        }
    }
}
