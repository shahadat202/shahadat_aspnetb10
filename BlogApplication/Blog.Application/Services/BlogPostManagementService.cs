using Blog.Domain.Entities;

namespace Blog.Application.Services
{
    public class BlogPostManagementService : IBlogPostManagementService
    {
        private readonly IBlogUnitOfWork _blogUnitOfWork;
        public BlogPostManagementService(IBlogUnitOfWork blogUnitOfWork)
        {
            _blogUnitOfWork = blogUnitOfWork;
        }
        public void CreateBlogPost(BlogPost blogPost)
        {
            _blogUnitOfWork.BlogPostRepository.Add(blogPost);
            _blogUnitOfWork.Save();
        }
    }
}
