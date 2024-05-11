using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using News.Web.Data;
using News.Web.Models.Domain;
using News.Web.Models.ViewModels;
using News.Web.Repositories;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.InteropServices;

namespace News.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminTagsController : Controller
    {
        private readonly ITagRepository tagRepository;

        public AdminTagsController(ITagRepository tagRepository)
        {
            this.tagRepository = tagRepository;
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddTagRequest addTagRequest)
        {
            var tags = await tagRepository.GetAllAsync();
            foreach(var tag in tags)
            {
                if(tag.Name == addTagRequest.Name)
                {
                    ModelState.AddModelError("Name", "Tên thẻ đã tồn tại");
                }
            }

            if (ModelState.IsValid)
            {
                var tag = new Tag
                {
                    Name = addTagRequest.Name,
                    DisplayName = addTagRequest.DisplayName,
                };

                await tagRepository.AddAsync(tag);
                return RedirectToAction("List");
            }

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> List()
        {
            var tags = await tagRepository.GetAllAsync();
            return View(tags);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {

            var tag = await tagRepository.GetAsync(id);
            if(tag!=null)
            {
                var editTagRequest = new EditTagRequest
                {
                    Id = tag.Id,
                    Name = tag.Name,
                    DisplayName = tag.DisplayName,
                };

                return View(editTagRequest);

            }
            return View(null);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditTagRequest editTagRequest)
        {
            var tags = await tagRepository.GetAllAsync();
            foreach (var item in tags)
            {
                if (item.Name == editTagRequest.Name)
                {
                    ModelState.AddModelError("Name", "Tên thẻ đã tồn tại");
                }
            }

            if(ModelState.IsValid)
            {
                var tag = new Tag
                {
                    Id = editTagRequest.Id,
                    Name = editTagRequest.Name,
                    DisplayName = editTagRequest.DisplayName,
                };

                if (tag != null)
                {
                    await tagRepository.UpdateAsync(tag);
                    return RedirectToAction("List");
                }
            }

            return View(editTagRequest);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(EditTagRequest editTagRequest)
        {
            var tag = await tagRepository.DeleteAsync(editTagRequest.Id);
            if(tag != null)
            {
                return RedirectToAction("List");
            }
            return RedirectToAction("Edit", new {id = editTagRequest.Id});

        }

        
    }
}
