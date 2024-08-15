using Autofac;
using Blog.Web.Models;

namespace Blog.Web
{
    public class WebModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<MemberShip>().As<IMemberShip>()
                .InstancePerLifetimeScope();
        }
    }
}
