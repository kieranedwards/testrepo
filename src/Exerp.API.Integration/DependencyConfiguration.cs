using System.Net;
using Autofac;
using Exerp.Api.Interfaces.Configuration;

namespace Exerp.API.Integration
{
    public class DependencyConfiguration
    {
        public static IContainer RegisterDependencies()
        {
           var builder = new ContainerBuilder();
           builder.RegisterModule(new Api.DependencyConfiguration(new IntegrationSettings())); 
           return builder.Build();
        }
    }

    public class IntegrationSettings : IExerpConfiguration
    {
        public string ExerpApiBaseUrl { get { return "https://puregym-test.exerp.com/api-v4/"; } }
        public NetworkCredential ExerpApiCredentials {
            get { return new NetworkCredential("100emp11801", "7483hfehffkTe843"); }
        }

        public int ExerpPureRideScopeId
        {
            get { return 80; } 
        }

        public int ExerpPureRidePrimaryCentreId
        {
            get { return 701; }
        }

    }
}