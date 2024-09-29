using DevSkill.Inventory.Domain.Entities;

namespace DevSkill.Inventory.Application.Services
{
    public class ProductManagementService : IProductManagementService
    {
        private readonly IInventoryUnitOfWork _InventoryUnitOfWork;
        public ProductManagementService(IInventoryUnitOfWork InventoryUnitOfWork)
        {
            _InventoryUnitOfWork = InventoryUnitOfWork;
        }

        public void InsertProduct(Product product)
        {
            _InventoryUnitOfWork.ProductRepository.Add(product);
            _InventoryUnitOfWork.Save();
        }

        public IEnumerable<Product> GetAllProducts()
        {
            return _InventoryUnitOfWork.ProductRepository.GetAllProducts();
        }
        
        public void DeleteProduct(Guid id)
        {
            _InventoryUnitOfWork.ProductRepository.Remove(id);
            _InventoryUnitOfWork.Save();
        }
    }
}
 