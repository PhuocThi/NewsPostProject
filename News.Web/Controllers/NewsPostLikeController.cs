using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using News.Web.Models.Domain;
using News.Web.Models.ViewModels;
using News.Web.Repositories;

namespace News.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewsPostLikeController : ControllerBase
    {
        private readonly INewsPostLikeRepository newsPostLikeRepository;

        public NewsPostLikeController(INewsPostLikeRepository newsPostLikeRepository)
        {
            this.newsPostLikeRepository = newsPostLikeRepository;
        }

        [HttpPost]
        [Route("Add")]
        public async Task<IActionResult> AddLike([FromBody] AddLikeRequest addLikeRequest)
        {
            var model = new NewsPostLike
            {
                NewsPostId = addLikeRequest.NewsPostId,
                UserId = addLikeRequest.UserId,
            };

            await newsPostLikeRepository.AddLikeForNews(model);
            return Ok();
        }

        [HttpGet]
        [Route("{newsPostId:Guid}/totalLikes")]
        public async Task<IActionResult> GetTotalLikesForNews([FromRoute] Guid newsPostId)
        {
            var totalLikes = await newsPostLikeRepository.GetTotalLikes(newsPostId);
            return Ok(totalLikes);
        }
    }
}
