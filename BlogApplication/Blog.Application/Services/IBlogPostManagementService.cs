using Blog.Domain;
using Blog.Domain.Dtos;
using Blog.Domain.Entities;

namespace Blog.Application.Services
{
    public interface IBlogPostManagementService
    {
        void CreateBlogPost(BlogPost blogPost);
        void DeleteBlogPost(Guid id);
        Task<BlogPost> GetBlogPostAsync(Guid id);
        (IList<BlogPost> data, int total, int totalDisplay) GetBlogPosts(int pageIndex, int pageSize,
            DataTablesSearch search, string? order);

        Task<(IList<BlogPostDto> data, int total, int totalDisplay)> GetBlogPostsSP(int pageIndex, int pageSize,
            DataTablesSearch search, string? order);
        void UpdateBlogPost(BlogPost blog);
    }
}