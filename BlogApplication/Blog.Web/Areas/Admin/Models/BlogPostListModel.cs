using Blog.Domain;
using Blog.Domain.Entities;

namespace Blog.Web.Areas.Admin.Models
{
    public class BlogPostListModel : DataTables
    {
        internal object FormatTableData((IList<BlogPost> result, int total, int totalDisplay) data)
        {
            throw new NotImplementedException();
        }
    }
}
