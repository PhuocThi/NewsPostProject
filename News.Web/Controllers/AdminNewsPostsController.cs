using Microsoft.AspNetCore.Mvc;
using News.Web.Models.ViewModels;
using News.Web.Repositories;
using Microsoft.AspNetCore.Mvc.Rendering;
using News.Web.Models.Domain;
using Microsoft.AspNetCore.Authorization;

namespace News.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminNewsPostsController : Controller
    {
        private readonly ITagRepository tagRepository;
        private readonly INewsPostRepository newsPostRepository;

        public AdminNewsPostsController(ITagRepository tagRepository, INewsPostRepository newsPostRepository)
        {
            this.tagRepository = tagRepository;
            this.newsPostRepository = newsPostRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            var tags = await tagRepository.GetAllAsync();
            var model = new AddNewsPostRequest
            {
                Tags = tags.Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                })
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddNewsPostRequest addNewsPostRequest)
        {
            
                var newsPost = new NewsPost
                {
                    Heading = addNewsPostRequest.Heading,
                    PageTitle = addNewsPostRequest.PageTitle,
                    Content = addNewsPostRequest.Content,
                    ShortDescription = addNewsPostRequest.ShortDescription,
                    FeaturedImageUrl = addNewsPostRequest.FeaturedImageUrl,
                    UrlHandle = addNewsPostRequest.UrlHandle,
                    PublishedDate = addNewsPostRequest.PublishedDate,
                    Author = addNewsPostRequest.Author,
                    Visible = addNewsPostRequest.Visible,

                };

                var selectedTags = new List<Tag>();

                foreach (var selectedTagId in addNewsPostRequest.SelectedTags)
                {
                    var selectedTagIdAsGuid = Guid.Parse(selectedTagId);
                    var existingTag = await tagRepository.GetAsync(selectedTagIdAsGuid);

                    if (existingTag != null)
                    {
                        selectedTags.Add(existingTag);
                    }
                }

                newsPost.Tags = selectedTags;

                await newsPostRepository.AddAsync(newsPost);
                return RedirectToAction("List");
            

           
        }

        [HttpGet]
        public async Task<IActionResult> List()
        {
            var newsPosts = await newsPostRepository.GetAllAsync();
            return View(newsPosts);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            
                var newsPost = await newsPostRepository.GetAsync(id);
                var tagsDomainModel = await tagRepository.GetAllAsync();

                if (newsPost != null)
                {
                    var model = new EditNewsPostRequest
                    {
                        Id = newsPost.Id,
                        Heading = newsPost.Heading,
                        PageTitle = newsPost.PageTitle,
                        Content = newsPost.Content,
                        Author = newsPost.Author,
                        FeaturedImageUrl = newsPost.FeaturedImageUrl,
                        UrlHandle = newsPost.UrlHandle,
                        ShortDescription = newsPost.ShortDescription,
                        PublishedDate = newsPost.PublishedDate,
                        Visible = newsPost.Visible,
                        Tags = tagsDomainModel.Select(x => new SelectListItem
                        {
                            Text = x.Name,
                            Value = x.Id.ToString()
                        }),
                        SelectedTags = newsPost.Tags.Select(x => x.Id.ToString()).ToArray(),
                    };
                    return View(model);
                }
            

            return RedirectToAction("Edit", new {id = id});
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditNewsPostRequest editNewsPostRequest)
        {
            var newsPostDomainModel = new NewsPost
            {
                Id = editNewsPostRequest.Id,
                Heading = editNewsPostRequest.Heading,
                Content = editNewsPostRequest.Content,
                Author = editNewsPostRequest.Author,
                FeaturedImageUrl = editNewsPostRequest.FeaturedImageUrl,
                PageTitle = editNewsPostRequest.PageTitle,
                PublishedDate = editNewsPostRequest.PublishedDate,
                ShortDescription = editNewsPostRequest.ShortDescription,
                UrlHandle = editNewsPostRequest.UrlHandle,
                Visible = editNewsPostRequest.Visible,

            };

            var selectedTags = new List<Tag>();
            foreach(var selectedTag in editNewsPostRequest.SelectedTags)
            {
                if(Guid.TryParse(selectedTag, out var tag))
                {
                    var foundTag = await tagRepository.GetAsync(tag);

                    if(foundTag != null)
                    {
                        selectedTags.Add(foundTag);
                    }
                }
            }

            newsPostDomainModel.Tags = selectedTags;

            var updatedNewsPost = await newsPostRepository.UpdateAsync(newsPostDomainModel);

            if(updatedNewsPost != null)
            {
                return RedirectToAction("List");
            }
            return RedirectToAction("Edit", new { id = editNewsPostRequest.Id });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(EditNewsPostRequest editNewsPostRequest)
        {
            var deletedNewsPost = await newsPostRepository.DeleteAsync(editNewsPostRequest.Id);
            if(deletedNewsPost!= null)
            {
                return RedirectToAction("List");
            }
            return RedirectToAction("Edit", new {id = editNewsPostRequest.Id});
        }
    }
}
