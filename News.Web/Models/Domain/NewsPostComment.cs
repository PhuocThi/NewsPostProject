namespace News.Web.Models.Domain
{
    public class NewsPostComment
    {
        public Guid Id { get; set; }
        public string Description { get; set; }
        public Guid NewsPostId { get; set; }
        public Guid UserId { get; set; }
        public DateTime DateAdded { get; set; }
    }
}
