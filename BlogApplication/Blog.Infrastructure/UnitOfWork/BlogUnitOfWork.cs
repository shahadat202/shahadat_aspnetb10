using Blog.Domain.RepositoryContracts;
using Microsoft.EntityFrameworkCore;
using Blog.Infrastructure.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blog.Infrastructure;
using Blog.Application;

namespace Blog.Infrustructure.UnitOfWorks
{
    public class BlogUnitOfWork : UnitOfWork , IBlogUnitOfWork
    {
        public IBlogPostRepository BlogPostRepository { get; private set; }
        public BlogUnitOfWork(BlogDbContext dbContext,
            IBlogPostRepository blogPostRepository) : base(dbContext)
        {
            BlogPostRepository = blogPostRepository;
        }
    }
}
