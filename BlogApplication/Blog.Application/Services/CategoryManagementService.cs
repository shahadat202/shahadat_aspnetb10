using Blog.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Application.Services
{
    public class CategoryManagementService : ICategoryManagementService
    {
        private readonly IBlogUnitOfWork _blogUnitOfWork;
        public CategoryManagementService(IBlogUnitOfWork blogUnitOfWork)
        {
            _blogUnitOfWork = blogUnitOfWork;
        }

        public IList<Category> GetCategories()
        {
            return _blogUnitOfWork.CategoryRepository.GetAll();
        }
    }
}