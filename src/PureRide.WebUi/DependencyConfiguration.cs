using System.Web.Http;
using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using PureRide.Web.Configuration;
using Umbraco.Web;

namespace PureRide.WebUI
{
    public class DependencyConfiguration
    {
        /// <remarks>
        /// Using AuoFaq for more details see https://our.umbraco.org/documentation/master/Reference/Mvc/using-ioc
        /// </remarks>
        public static void RegisterDependencies()
        {
            var builder = new ContainerBuilder();
            var settings = new WebSiteSettings();

            builder.RegisterModule(new Exerp.Api.DependencyConfiguration(settings));

            if (UseOfflineApi())
                builder.RegisterModule(new Exerp.Api.Offline.DependencyConfiguration());

            builder.RegisterModule(new SagePay.DependencyConfiguration());
            builder.RegisterModule(new Data.DependencyConfiguration(new WebSiteSettings()));
            builder.RegisterModule(new Web.DependencyConfiguration());
            builder.RegisterApiControllers(typeof(UmbracoApplication).Assembly);

            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

            var resolver = new AutofacWebApiDependencyResolver(container);
            GlobalConfiguration.Configuration.DependencyResolver = resolver;
        }

        private static bool UseOfflineApi()
        {
            return WebSiteSettings.GetSetting<bool>("Exerp.UseOfflineApi");
        }
    }
}