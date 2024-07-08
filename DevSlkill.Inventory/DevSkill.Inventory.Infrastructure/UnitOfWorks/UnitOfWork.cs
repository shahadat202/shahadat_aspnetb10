﻿using DevSkill.Inventory.Domain;
using DevSkill.Inventory.Domain.RepositoryContracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Infrastructure.UnitOfWorks
{
    public abstract class UnitOfWork : IUnitOfWork
    {
        private readonly DbContext _dbContext;
        //protected IAdoNetUtility AdoNetUtility { get; private set; }

        public UnitOfWork(DbContext dbContext)
        {
            _dbContext = dbContext;
            //AdoNetUtility = new AdoNetUtility(_dbContext.Database.GetDbConnection());
        }

        public void Dispose() => _dbContext?.Dispose();
        public ValueTask DisposeAsync() => _dbContext.DisposeAsync();
        public void Save() => _dbContext?.SaveChanges();
        public async Task SaveAsync() => await _dbContext.SaveChangesAsync();
    }
}
