﻿using Blog.Domain.RepositoryContracts;
using Microsoft.EntityFrameworkCore;
using Blog.Infrastructure.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blog.Infrastructure;
using Blog.Application;
using Blog.Domain;
using Blog.Domain.Entities;
using Blog.Domain.Dtos;

namespace Blog.Infrustructure.UnitOfWorks
{
    public class BlogUnitOfWork : UnitOfWork , IBlogUnitOfWork
    {
        public IBlogPostRepository BlogPostRepository { get; private set; }
        public ICategoryRepository CategoryRepository { get; private set; }
        public BlogUnitOfWork(BlogDbContext dbContext,
            IBlogPostRepository blogPostRepository,
            ICategoryRepository categoryRepository) : base(dbContext)
        {
            BlogPostRepository = blogPostRepository;
            CategoryRepository = categoryRepository;
        }
        public async Task<(IList<BlogPostDto> data, int total, int totalDisplay)> GetPagedBlogPostsUsingSPAsync(int pageIndex,
            int pageSize, BlogPostSearchDto search, string? order)
        {
            var procedureName = "GetBlogPosts";

            var result = await SqlUtility.QueryWithStoredProcedureAsync<BlogPostDto>(procedureName,
                new Dictionary<string, object>
                {
                    { "PageIndex", pageIndex },
                    { "PageSize", pageSize },
                    { "OrderBy", order },
                    { "CreateDateFrom", DateTime.Parse(search.CreateDateFrom) },
                    { "CreateDateTo", DateTime.Parse(search.CreateDateTo) },
                    { "Title", search.Title == string.Empty ? null : search.Title },
                    { "CategoryId", search.CategoryId == Guid.Empty ? null : search.CategoryId}
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
