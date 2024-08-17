﻿using Blog.Domain;
using Blog.Domain.RepositoryContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Application
{
    public interface IBlogUnitOfWork : IUnitOfWork
    {
        public IBlogPostRepository BlogPostRepository { get; }
    }
}
