using Autofac;
using Blog.Application;
using Blog.Application.Services;
using Blog.Domain.RepositoryContracts;
using Blog.Infrastructure;
using Blog.Infrastructure.Repositories;
using Blog.Infrustructure.Repositories;
using Blog.Infrustructure.UnitOfWorks;
using Blog.Web.Data;
using Blog.Web.Models;

namespace Blog.Web
{
    public class WebModule(string connectionString, string? migrationAssembly) : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<BlogDbContext>().AsSelf()
                .WithParameter("connectionString", connectionString)
                .WithParameter("migrationAssembly", migrationAssembly)
                .InstancePerLifetimeScope();

            builder.RegisterType<BlogPostManagementService>()
                .As<IBlogPostManagementService>()
                .InstancePerLifetimeScope();

            builder.RegisterType<BlogPostRepository>()
                .As<IBlogPostRepository>()
                .InstancePerLifetimeScope();

            builder.RegisterType<BlogUnitOfWork>()
                .As<IBlogUnitOfWork>()
                .InstancePerLifetimeScope();

            builder.RegisterType<ApplicationDbContext>().AsSelf()
                .InstancePerLifetimeScope();

        }
    }
}
