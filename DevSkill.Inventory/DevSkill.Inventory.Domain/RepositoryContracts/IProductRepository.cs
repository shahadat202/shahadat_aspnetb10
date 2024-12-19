using DevSkill.Inventory.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Domain.RepositoryContracts
{
    public interface IProductRepository : IRepositoryBase<Product, Guid>
    {
        Task<IEnumerable<Product>> GetAllProductsAsync();
        bool IsTitleDuplicate(string title, Guid? id = null);
        Task<Product> GetByIdAsync(Guid productId);
    }
}
