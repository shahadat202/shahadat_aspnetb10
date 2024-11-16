using DevSkill.Inventory.Domain.Dtos;
using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Domain.RepositoryContracts;

namespace DevSkill.Inventory.Application.Services
{
    public class ProductManagementService : IProductManagementService
    {
        private readonly IInventoryUnitOfWork _InventoryUnitOfWork;
        private readonly IActivityLogRepository _ActivityLogRepository;
        public ProductManagementService(IInventoryUnitOfWork InventoryUnitOfWork,
            IActivityLogRepository activityLogRepository)
        {
            _InventoryUnitOfWork = InventoryUnitOfWork;
            _ActivityLogRepository = activityLogRepository;

        }

        public void InsertProduct(Product product, string username)
        {
            _InventoryUnitOfWork.ProductRepository.Add(product);
            _ActivityLogRepository.Add(new ActivityLog()
            {
                Username = username,
                Action = "Inserted",
                ItemName = product.Title,
                ActionDate = DateTime.Now,
            });
            _InventoryUnitOfWork.Save();
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            return await _InventoryUnitOfWork.ProductRepository.GetAllProductsAsync();
        }

        public async Task<Product> GetProductAsync(Guid id)
        {
            return await _InventoryUnitOfWork.ProductRepository.GetByIdAsync(id);
        }

        public void UpdateProduct(Product product, string username)
        {
            if (!_InventoryUnitOfWork.ProductRepository.IsTitleDuplicate(product.Title, product.Id))
            {
                _InventoryUnitOfWork.ProductRepository.Edit(product);
                _ActivityLogRepository.Add(new ActivityLog()
                {
                    Username = username,
                    Action = "Updated",
                    ItemName = product.Title,
                    ActionDate = DateTime.Now,
                });
                _InventoryUnitOfWork.Save();
            }
            else
                throw new InvalidOperationException("Title should be unique.");
        }

        public void DeleteProduct(Guid id, string username)
        {
            var product = _InventoryUnitOfWork.ProductRepository.GetById(id);
            if (product == null)
            {
                throw new InvalidOperationException("Product not found");
            }

            // Remove the product
            _InventoryUnitOfWork.ProductRepository.Remove(id);
            _ActivityLogRepository.Add(new ActivityLog()
            {
                Username = username,
                Action = "Deleted",
                ItemName = product.Title,
                ActionDate = DateTime.Now,
            });
            _InventoryUnitOfWork.Save();
        }

        public async Task<(IList<ProductDto> data, int total, int totalDisplay)> GetProductsSP(int pageIndex, 
            int pageSize, ProductSearchDto search, string? order)
        {
            return await _InventoryUnitOfWork.GetPagedProductsUsingSPAsync(pageIndex, pageSize, search, order);
        }
    }
}
 