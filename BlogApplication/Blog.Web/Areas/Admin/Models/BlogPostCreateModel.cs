using Blog.Domain.Entities;
using Blog.Infrastructure;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Blog.Web.Areas.Admin.Models
{
    public class BlogPostCreateModel
    {
        [Required, StringLength(100)]
        public string Title { get; set; }
        [Required, StringLength(100)]
        public string Body { get; set; }
        [Display(Name = "Category"), Required]
        public Guid CategoryId { get; set; }
        public IList<SelectListItem>? Categories { get; private set; }

        public void SetCategoryValues(IList<Category> categories)
        {
            Categories = RazorUtility.ConvertCategories(categories);
        }
    }
}
