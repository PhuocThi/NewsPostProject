using News.Web.Models.Domain;

namespace News.Web.Models.ViewModels
{
    public class HomeViewModel
    {
        public IEnumerable<NewsPost> NewsPosts { get; set; }
        public IEnumerable<Tag> Tags { get; set; }
    }
}
