using Blog.Application.Services;
using Blog.Domain.Entities;
using Blog.Web.Areas.Admin.Models;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace Blog.Web.Areas.Admin.Controllers
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
