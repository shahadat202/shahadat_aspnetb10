using DevSkill.Inventory.Web.Areas.Admin.Models;
using Inventory.Application.Services;
using Inventory.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace DevSkill.Inventory.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BlogPostController : Controller
    {
        private readonly IBlogPostManagementService _blogPostManagementService;
        public BlogPostController(IBlogPostManagementService blogPostManagementService)
        {
            _blogPostManagementService = blogPostManagementService;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Create()
        {
            var model = new BlogPostCreateModel();
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Create(BlogPostCreateModel model)
        {
            if (ModelState.IsValid)
            {
                var blog = new BlogPost { Title = model.Title };

                _blogPostManagementService.CreateBlogPost(blog);
                return RedirectToAction("Index");
            }
            return View();
        }
    }
}
