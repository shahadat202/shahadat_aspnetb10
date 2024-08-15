using Blog.Domain.Entities;

namespace Blog.Application.Services
{
    public interface IBlogPostManagementService
    {
        void CreateBlogPost(BlogPost blogPost);
    }
}