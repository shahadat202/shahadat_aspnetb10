using Blog.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Domain.RepositoryContracts
{
    public interface IBlogPostRepository : IRepositoryBase<BlogPost, Guid>
    {
        (IList<BlogPost> data, int total, int totalDisplay) GetPagedBlogPosts(int pageIndex,
            int pageSize, DataTablesSearch search, string? order);

        //bool IsTitleDuplicate(string title, Guid? id = null);
    }
}
