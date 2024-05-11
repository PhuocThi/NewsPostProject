namespace News.Web.Models.ViewModels
{
    public class AddLikeRequest
    {
        public Guid NewsPostId { get; set; }
        public Guid UserId { get; set; }
    }
}
