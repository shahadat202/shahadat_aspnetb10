﻿using DevSkill.Inventory.Domain;
using DevSkill.Inventory.Domain.RepositoryContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Application
{
    public interface IProductUnitOfWork : IUnitOfWork
    {
        public IProductRepository ProductRepository { get; }
    }
}
