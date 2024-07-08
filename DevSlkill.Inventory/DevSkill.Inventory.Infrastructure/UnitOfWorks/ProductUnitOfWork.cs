using DevSkill.Inventory.Domain.RepositoryContracts;
using DevSkill.Inventory.Infrastructure.UnitOfWorks;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Infrastructure.UnitOfWorks
{
    public class ProductUnitOfWork : UnitOfWork
    {
        public IProductRepository ProductRepository { get; set; }
        public ProductUnitOfWork(ProductDbContext dbContext, 
            IProductRepository productRepository) : base(dbContext)
        {
            ProductRepository = productRepository;
        }
    }
}
