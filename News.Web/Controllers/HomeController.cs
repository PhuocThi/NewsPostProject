using Microsoft.AspNetCore.Mvc;
using News.Web.Models;
using News.Web.Models.ViewModels;
using News.Web.Repositories;
using System.Diagnostics;

namespace News.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly INewsPostRepository newsPostRepository;
        private readonly ITagRepository tagRepository;

        public HomeController(ILogger<HomeController> logger, INewsPostRepository newsPostRepository
                             ,ITagRepository tagRepository)
        {
            _logger = logger;
            this.newsPostRepository = newsPostRepository;
            this.tagRepository = tagRepository;
        }

        public async Task<IActionResult> Index()
        {
            var newsPosts = await newsPostRepository.GetAllAsync();

            var tags = await tagRepository.GetAllAsync();

            var model = new HomeViewModel
            {
                NewsPosts = newsPosts,
                Tags = tags
            };

            return View(model);
        }

        
    }
}
