using Blog.Domain;
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
            if (!_blogUnitOfWork.BlogPostRepository.IsTitleDuplicate(blogPost.Title))
            {
                _blogUnitOfWork.BlogPostRepository.Add(blogPost);
                _blogUnitOfWork.Save();
            }
        }

        public void DeleteBlogPost(Guid id)
        {
            _blogUnitOfWork.BlogPostRepository.Remove(id);
            _blogUnitOfWork.Save();
        }

        public (IList<BlogPost> data, int total, int totalDisplay) GetBlogPosts(int pageIndex,
            int pageSize, DataTablesSearch search, string? order)
        {
            return _blogUnitOfWork.BlogPostRepository.GetPagedBlogPosts(pageIndex, pageSize, search, order);
        }

        public BlogPost GetBlogPost(Guid id)
        {
            return _blogUnitOfWork.BlogPostRepository.GetById(id);
        }

        public void UpdateBlogPost(BlogPost blogPost)
        {
            if (!_blogUnitOfWork.BlogPostRepository.IsTitleDuplicate(blogPost.Title, blogPost.Id))
            {
                _blogUnitOfWork.BlogPostRepository.Edit(blogPost);
                _blogUnitOfWork.Save();
            }
            else
            {
                throw new InvalidOperationException("Title should be unique.");
            }
        }
    }
}
