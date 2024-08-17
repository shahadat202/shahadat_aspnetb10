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
    }
}
