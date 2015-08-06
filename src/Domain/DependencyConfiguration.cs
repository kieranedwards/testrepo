using Autofac;
using Exerp.Api.Interfaces;

namespace PureRide.Domain
{
    public class DependencyConfiguration : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
           //builder.Register(c => new AccountService(c.Resolve<ISelfServiceApi>())).As<IAccountService>();
        }
    }
}
