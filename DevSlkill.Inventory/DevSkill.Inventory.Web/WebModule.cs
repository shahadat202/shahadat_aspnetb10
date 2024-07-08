﻿using Autofac;
using DevSkill.Inventory.Application.Services;
using DevSkill.Inventory.Domain.RepositoryContracts;
using DevSkill.Inventory.Infrastructure;
using DevSkill.Inventory.Infrastructure.Repositories;
using DevSkill.Inventory.Web.Models;
public class WebModule(string connectionString, string migrationAssembly) : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<EmailSender>().As<IEmailSender>()
            .InstancePerLifetimeScope();

        builder.RegisterType<ProductManagementService>()
            .As<IProductManagementService>()
            .InstancePerLifetimeScope();

        builder.RegisterType<ProductDbContext>().AsSelf()
            .WithParameter("connectionString", connectionString)
            .WithParameter("migrationAssembly", migrationAssembly)
            .InstancePerLifetimeScope();

        builder.RegisterType<ProductRepository>()
            .As<IProductRepository>()
            .InstancePerLifetimeScope();
    }
}
