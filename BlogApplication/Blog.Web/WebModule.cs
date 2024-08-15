using Autofac;
using Blog.Application.Services;
using Blog.Web.Models;

namespace Blog.Web
{
    public class WebModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<MemberShip>().As<IMemberShip>()
                .InstancePerLifetimeScope();

            builder.RegisterType<BlogPostManagementService>()
                .As<IBlogPostManagementService>()
                .InstancePerLifetimeScope();
        }
    }
}
