using Blog.Domain;
using Blog.Domain.Dtos;
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

        public async Task<(IList<BlogPostDto> data, int total, int totalDisplay)> GetBlogPostsSP(int pageIndex,
            int pageSize, BlogPostSearchDto search, string? order)
        {
            return await _blogUnitOfWork.GetPagedBlogPostsUsingSPAsync(pageIndex, pageSize, search, order);
        }

        public async Task<BlogPost> GetBlogPosts(Guid id)
        {
            return await _blogUnitOfWork.BlogPostRepository.GetBlogPostAsync(id);
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

        public async Task<BlogPost> GetBlogPostAsync(Guid id)
        {
            return await _blogUnitOfWork.BlogPostRepository.GetBlogPostAsync(id);
        }
    }
}
