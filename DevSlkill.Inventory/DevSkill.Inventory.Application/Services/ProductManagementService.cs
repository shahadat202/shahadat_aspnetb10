using DevSkill.Inventory.Domain.Entities;

namespace DevSkill.Inventory.Application.Services
{
    public class ProductManagementService : IProductManagementService
    {
        private readonly IProductUnitOfWork _productUnitOfWork;
        public ProductManagementService(IProductUnitOfWork productUnitOfWork)
        {
            _productUnitOfWork = productUnitOfWork;
        }
        public void InsertProduct(Product product)
        {
            _productUnitOfWork.ProductRepository.Add(product);
            _productUnitOfWork.Save();
        }
    }
}
 