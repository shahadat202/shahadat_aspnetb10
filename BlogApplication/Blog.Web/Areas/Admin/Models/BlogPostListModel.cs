using Blog.Domain;
using Blog.Domain.Dtos;
using Blog.Domain.Entities;
using Blog.Infrastructure;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Blog.Web.Areas.Admin.Models
{
    public class BlogPostListModel : DataTables
    {
        //internal object FormatTableData((IList<BlogPost> result, int total, int totalDisplay) data)
        //{
        //    throw new NotImplementedException();
        //}

        public BlogPostSearchDto SearchItem { get; set; }

        public IList<SelectListItem> Categories { get; private set; }

        public void SetCategoryValues(IList<Category> categories)
        {
            Categories = RazorUtility.ConvertCategories(categories);
        }
    }
}
