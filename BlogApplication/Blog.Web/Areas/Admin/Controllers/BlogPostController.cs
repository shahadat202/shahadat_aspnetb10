using Blog.Application.Services;
using Blog.Domain.Entities;
using Blog.Web.Areas.Admin.Models;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using System.Web;

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

        public JsonResult GetBlogPostJsonData(BlogPostListModel model)
        {
            var result = _blogPostManagementService.GetBlogPosts(model.PageIndex, model.PageSize, model.Search,
                model.FormatSortExpression("Title"));

            var blogPostJsonData = new
            {
                recordsTotal = result.total,
                recordsFiltered = result.totalDisplay,
                data = (from record in result.data
                        select new string[]
                        {
                            HttpUtility.HtmlEncode(record.Title),
                            record.Id.ToString()
                        }
                    ).ToArray()
            };
            return Json(blogPostJsonData);
        }

        public IActionResult Create()
        {
            var model = new BlogPostCreateModel();
            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Create(BlogPostCreateModel model)
        {
            if (ModelState.IsValid)
            {
                var blog = new BlogPost { Id = Guid.NewGuid(), Title = model.Title };
                _blogPostManagementService.CreateBlogPost(blog);
                return RedirectToAction("Index");
            }
            return View();
        }
    }
}
