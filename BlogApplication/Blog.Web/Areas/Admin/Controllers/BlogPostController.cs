using Blog.Application.Services;
using Blog.Domain.Entities;
using Blog.Web.Areas.Admin.Models;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using System.Web;
using Blog.Infrastructure;

namespace Blog.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BlogPostController : Controller
    {
        private readonly IBlogPostManagementService _blogPostManagementService;
        private readonly ILogger<BlogPostController> _logger;
        public BlogPostController(IBlogPostManagementService blogPostManagementService
            , ILogger<BlogPostController> logger)
        {
            _blogPostManagementService = blogPostManagementService;
            _logger = logger;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetBlogPostJsonData([FromBody] BlogPostListModel model)
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

                try
                {
                    TempData.Put("ResponseMessage", new ResponseModel
                    {
                        Message = "Blog post created successfully!",
                        Type = ResponseTypes.Success
                    });
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    TempData.Put("ResponseMessage", new ResponseModel
                    {
                        Message = "Blog post creation failed!",
                        Type = ResponseTypes.Danger
                    });
                    _logger.LogError(ex, "Blog post creation failed!");
                }
            }
            return View();
        }

        public IActionResult Update(Guid id)
        {
            var model = new BlogPostUpdateModel();

            BlogPost post = _blogPostManagementService.GetBlogPost(id);
            model.Title = post.Title;
            model.Id = post.Id;

            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Update(BlogPostUpdateModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var blog = new BlogPost { Id = model.Id, Title = model.Title };
                    _blogPostManagementService.UpdateBlogPost(blog);

                    TempData.Put("ResponseMessage", new ResponseModel
                    {
                        Message = "Blog post updated successfully!",
                        Type = ResponseTypes.Success
                    });
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    TempData.Put("ResponseMessage", new ResponseModel
                    {
                        Message = "Blog post update failed! There is another entity with this name.",
                        Type = ResponseTypes.Danger
                    });
                    _logger.LogError(ex, "Blog post update failed!");
                }
            }
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Delete(Guid id)
        {
            try
            {
                _blogPostManagementService.DeleteBlogPost(id);

                TempData.Put("ResponseMessage", new ResponseModel
                {
                    Message = "Blog post deleted successfully!",
                    Type = ResponseTypes.Success
                });
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData.Put("ResponseMessage", new ResponseModel
                {
                    Message = "Blog post delete failed!",
                    Type = ResponseTypes.Danger
                });
                _logger.LogError(ex, "Blog post delete failed!");
            }
            return View();
        }
    }
}
