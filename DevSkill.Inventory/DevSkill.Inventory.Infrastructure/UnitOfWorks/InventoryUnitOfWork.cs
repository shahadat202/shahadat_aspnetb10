using DevSkill.Inventory.Application;
using DevSkill.Inventory.Domain.Dtos;
using DevSkill.Inventory.Domain.RepositoryContracts;
using DevSkill.Inventory.Infrastructure.UnitOfWorks;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Infrastructure.UnitOfWorks
{
    public class InventoryUnitOfWork : UnitOfWork, IInventoryUnitOfWork
    {
        public IProductRepository ProductRepository { get; private set; }
        public InventoryUnitOfWork(InventoryDbContext dbContext, 
            IProductRepository productRepository) : base(dbContext)
        {
            ProductRepository = productRepository;
        }
        public async Task<(IList<ProductDto> data, int total, int totalDisplay)> GetPagedProductsUsingSPAsync(int pageIndex,
            int pageSize, ProductSearchDto search, string? order)
        {
            var procedureName = "GetProducts";

            var result = await SqlUtility.QueryWithStoredProcedureAsync<ProductDto>(procedureName,
                new Dictionary<string, object>
                {
                    { "PageIndex", pageIndex },
                    { "PageSize", pageSize },
                    { "OrderBy", order },
                    { "Title", string.IsNullOrEmpty(search.Title) ? null : search.Title },
                    { "Quantity", search.Quantity },
                    { "MinLevel", search.MinLevel },
                    { "Tag", string.IsNullOrEmpty(search.Tag) ? null : search.Tag },
                    { "PriceFrom", search.PriceFrom },
                    { "PriceTo", search.PriceTo },
                    { "DateFrom", search.DateFrom.HasValue ? search.DateFrom.Value : (object)DBNull.Value },
                    { "DateTo", search.DateTo.HasValue ? search.DateTo.Value : (object)DBNull.Value }
                },
                new Dictionary<string, Type>
                {
                    { "Total", typeof(int) },
                    { "TotalDisplay", typeof(int) },
                });

            return (result.result, (int)result.outValues["Total"], (int)result.outValues["TotalDisplay"]);
        }

    }
}
