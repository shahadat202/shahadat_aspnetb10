using Blog.Domain;
using Blog.Domain.Dtos;
using Blog.Domain.Entities;
using Blog.Domain.RepositoryContracts;
using Blog.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Infrustructure.Repositories
{
    public class BlogPostRepository : Repository<BlogPost, Guid>, IBlogPostRepository
    {
        public BlogPostRepository(BlogDbContext context) : base(context)
        {

        }

        public bool IsTitleDuplicate(string title, Guid? id = null)
        {
            if (id.HasValue)
            {
                return GetCount(x => x.Id != id.Value && x.Title == title) > 0;
            }
            else
            {
                return GetCount(x => x.Title == title) > 0;
            }
        }

        public (IList<BlogPost> data, int total, int totalDisplay) GetPagedBlogPosts(int pageIndex, int pageSize,
            DataTablesSearch search, string? order)
        {
            if (string.IsNullOrWhiteSpace(search.Value))
                return GetDynamic(null, order, y => y.Include(z => z.Category), 
                    pageIndex, pageSize, true);
            else
                return GetDynamic(x => x.Title.Contains(search.Value), order,
                    y => y.Include(z => z.Category), pageIndex, pageSize, true);
        }

        public async Task<BlogPost> GetBlogPostAsync(Guid id)
        {
            return (await GetAsync(x => x.Id == id, y => y.Include(z => z.Category))).FirstOrDefault();
        }
    }
}
