using Autofac;
using DevSkill.Inventory.Web.Models;
public class WebModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<EmailSender>().As<IEmailSender>()
            .InstancePerLifetimeScope();
    }
}
