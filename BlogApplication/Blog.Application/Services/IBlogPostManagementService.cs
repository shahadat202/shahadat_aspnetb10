using Blog.Domain;
using Blog.Domain.Entities;

namespace Blog.Application.Services
{
    public interface IBlogPostManagementService
    {
        void CreateBlogPost(BlogPost blogPost);
        void DeleteBlogPost(Guid id);
        (IList<BlogPost> data, int total, int totalDisplay) GetBlogPosts(int pageIndex,
            int pageSize, DataTablesSearch search, string? order);
        BlogPost GetBlogPosts(Guid id);
        //void UpdateBlogPost(BlogPost blog);
    }

}