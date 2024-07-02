using Inventory.Domain.RepositoryContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Infrastructure.UnitOfWorks
{
    public class BlogUnitOfWork : UnitOfWork
    {
        public IBlogPostRepository BlogPostRepository { get; private set; }
        public BlogUnitOfWork(BlogDbContext dbContext,
            IBlogPostRepository blogPostRepository) : base(dbContext)
        {
            BlogPostRepository = blogPostRepository;
        }
    }
}
