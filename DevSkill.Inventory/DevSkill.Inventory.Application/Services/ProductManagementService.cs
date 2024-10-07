﻿using DevSkill.Inventory.Domain.Entities;

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

        public Product GetProduct(Guid id)
        {
            return _InventoryUnitOfWork.ProductRepository.GetById(id);
        }

        public void UpdateProduct(Product product)
        {
            if (!_InventoryUnitOfWork.ProductRepository.IsTitleDuplicate(product.Title, product.Id))
            {
                _InventoryUnitOfWork.ProductRepository.Edit(product);
                _InventoryUnitOfWork.Save();
            }
            else
                throw new InvalidOperationException("Title should be unique.");
        }

        public void DeleteProduct(Guid id)
        {
            _InventoryUnitOfWork.ProductRepository.Remove(id);
            _InventoryUnitOfWork.Save();  
        }
    }
}
 