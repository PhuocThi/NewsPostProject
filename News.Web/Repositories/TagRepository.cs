using Microsoft.EntityFrameworkCore;
using News.Web.Data;
using News.Web.Models.Domain;
using News.Web.Models.ViewModels;

namespace News.Web.Repositories
{
    public class TagRepository : ITagRepository
    {
        private readonly NewsDbContext newsDbContext;

        public TagRepository(NewsDbContext newsDbContext)
        {
            this.newsDbContext = newsDbContext;
        }
        public async Task<Tag> AddAsync(Tag tag)
        {
            await newsDbContext.Tags.AddAsync(tag);
            await newsDbContext.SaveChangesAsync();
            return tag;
        }

        public async Task<Tag?> DeleteAsync(Guid id)
        {
            var tag = await newsDbContext.Tags.FindAsync(id);
            if (tag != null)
            {
                newsDbContext.Tags.Remove(tag);
                await newsDbContext.SaveChangesAsync();
                return tag;
            }
            return null;
        }

        public async Task<IEnumerable<Tag>> GetAllAsync()
        {
            return await newsDbContext.Tags.ToListAsync();
        }

        public async Task<Tag?> GetAsync(Guid id)
        {
            return await newsDbContext.Tags.FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<Tag?> UpdateAsync(Tag tag)
        {
            var existingTag = await newsDbContext.Tags.FindAsync(tag.Id);

            if (existingTag != null)
            {
                existingTag.Name = tag.Name;
                existingTag.DisplayName = tag.DisplayName;

                await newsDbContext.SaveChangesAsync();

                return existingTag;
            }

            return null;
        }
    }
}
