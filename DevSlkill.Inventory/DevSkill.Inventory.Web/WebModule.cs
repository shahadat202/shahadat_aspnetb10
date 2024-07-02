using Autofac;
using DevSkill.Inventory.Web.Models;
using Inventory.Application.Services;
using Inventory.Domain.RepositoryContracts;
using Inventory.Infrastructure;
using Inventory.Infrastructure.Repositories;
public class WebModule(string connectionString, string migrationAssembly) : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<EmailSender>().As<IEmailSender>()
            .InstancePerLifetimeScope();

        builder.RegisterType<BlogDbContext>().AsSelf()
            .WithParameter("connectionString", connectionString)
            .WithParameter("migrationAssembly", migrationAssembly)
            .InstancePerLifetimeScope();

        builder.RegisterType<BlogPostRepository>()
            .As<IBlogPostRepository>()
            .InstancePerLifetimeScope();

        builder.RegisterType<BlogPostManagementService>()
            .As<IBlogPostManagementService>()
            .InstancePerLifetimeScope();
    }
}
