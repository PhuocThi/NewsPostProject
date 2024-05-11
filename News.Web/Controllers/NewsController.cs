using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using News.Web.Models.Domain;
using News.Web.Models.ViewModels;
using News.Web.Repositories;

namespace News.Web.Controllers
{
    public class NewsController : Controller
    {
        private readonly INewsPostRepository newsPostRepository;
        private readonly INewsPostLikeRepository newsPostLikeRepository;
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly UserManager<IdentityUser> userManager;
        private readonly INewsPostCommentRepository newsPostCommentRepository;

        public NewsController(INewsPostRepository newsPostRepository, 
                              INewsPostLikeRepository newsPostLikeRepository,
                              SignInManager<IdentityUser> signInManager,
                              UserManager<IdentityUser> userManager,
                              INewsPostCommentRepository newsPostCommentRepository)
        {
            this.newsPostRepository = newsPostRepository;
            this.newsPostLikeRepository = newsPostLikeRepository;
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.newsPostCommentRepository = newsPostCommentRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string urlHandle)
        {
            var liked = false;
            var newsPost = await newsPostRepository.GetByUrlHandleAsync(urlHandle);
            var newsPostLikeViewModel = new NewsDetailsViewModel();

            if (newsPost != null)
            {
                var totalLikes = await newsPostLikeRepository.GetTotalLikes(newsPost.Id);

                if(signInManager.IsSignedIn(User))
                {
                    var likesForNews = await newsPostLikeRepository.GetLikesForNews(newsPost.Id);

                    var userId = userManager.GetUserId(User);

                    if(userId != null)
                    {
                        var likeFromUser = likesForNews.FirstOrDefault(x=>x.UserId == Guid.Parse(userId));

                        liked = likeFromUser != null;
                    }

                }

                //get comments
                var newsCommentsDomainModel = await newsPostCommentRepository.GetCommentsByNewsIdAsync(newsPost.Id);

                var newsCommentsForView = new List<NewsComment>();

                foreach(var comment in newsCommentsDomainModel)
                {
                    newsCommentsForView.Add(new NewsComment
                    {
                        Description = comment.Description,
                        DateAdded = comment.DateAdded,
                        Username = (await userManager.FindByIdAsync(comment.UserId.ToString())).UserName
                    });
                }

                newsPostLikeViewModel = new NewsDetailsViewModel
                {
                    Id = newsPost.Id,
                    Author = newsPost.Author,
                    PageTitle = newsPost.PageTitle,
                    Content = newsPost.Content,
                    FeaturedImageUrl = newsPost.FeaturedImageUrl,
                    Heading = newsPost.Heading,
                    PublishedDate = newsPost.PublishedDate,
                    ShortDescription = newsPost.ShortDescription,
                    Tags = newsPost.Tags,
                    Visible = newsPost.Visible,
                    UrlHandle = newsPost.UrlHandle,
                    TotalLikes = totalLikes,
                    Liked = liked,
                    Comments = newsCommentsForView
                };
            }
            return View(newsPostLikeViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Index(NewsDetailsViewModel newsDetailsViewModel)
        {
            if (signInManager.IsSignedIn(User))
            {
                var domainModel = new NewsPostComment
                {
                    NewsPostId = newsDetailsViewModel.Id,
                    Description = newsDetailsViewModel.CommentDescription,
                    UserId = Guid.Parse(userManager.GetUserId(User)),
                    DateAdded = DateTime.Now
                };
                await newsPostCommentRepository.AddAsync(domainModel);
                return RedirectToAction("Index", "News", new {urlHandle = newsDetailsViewModel.UrlHandle});
            }

            return View();

        }
    }
}
