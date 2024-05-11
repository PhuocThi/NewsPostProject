namespace News.Web.Models.Domain
{
    public class NewsPostLike
    {
        public Guid Id { get; set; }
        public Guid NewsPostId { get; set; }
        public Guid UserId { get; set; }
    }
}
