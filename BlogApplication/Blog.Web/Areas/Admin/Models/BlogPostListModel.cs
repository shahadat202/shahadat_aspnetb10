using Blog.Domain;
using Blog.Domain.Dtos;
using Blog.Domain.Entities;
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
            Categories = (from c in categories
                          select new SelectListItem(c.Name, c.Id.ToString()))
                          .ToList();

            Categories.Insert(0, new SelectListItem("All", Guid.Empty.ToString()));
        }
    }
}
