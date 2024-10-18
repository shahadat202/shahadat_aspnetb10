﻿using DevSkill.Inventory.Domain.Dtos;
using DevSkill.Inventory.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Application.Services
{
    public interface IProductManagementService
    {
        void InsertProduct(Product product);
        IEnumerable<Product> GetAllProducts();
        Product GetProduct(Guid id);
        void UpdateProduct(Product product);
        void DeleteProduct(Guid id);

        Task<(IList<ProductDto> data, int total, int totalDisplay)> GetProductsSP(int pageIndex, int pageSize,
            ProductSearchDto search, string? order);
    }
}
