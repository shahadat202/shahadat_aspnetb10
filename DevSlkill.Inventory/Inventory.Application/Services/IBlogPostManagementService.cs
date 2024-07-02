using Inventory.Domain.Entities;

namespace Inventory.Application.Services
{
    public interface IBlogPostManagementService
    {
        void CreateBlogPost(BlogPost blogPost);
    }
}