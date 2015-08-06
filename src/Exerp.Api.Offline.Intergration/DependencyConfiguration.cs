using Autofac;

namespace Exerp.Api.Offline.Integration
{
    public class DependencyConfiguration
    {
        public static IContainer RegisterDependencies()
        {
           var builder = new ContainerBuilder();
           builder.RegisterModule(new Offline.DependencyConfiguration()); 
           return builder.Build();
        }
    }
}