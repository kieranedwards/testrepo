using Autofac;
using Exerp.Api.Helpers;
using Exerp.Api.Interfaces.Configuration;
using Exerp.Api.Interfaces.Services;
using Exerp.Api.Interfaces.WebServices;
using Exerp.Api.Services;
using Exerp.Api.WebServices;
using Exerp.Api.WebServices.BookingAPI;
using Exerp.Api.WebServices.PersonAPI;
using Exerp.Api.WebServices.SelfServiceAPI;
using Exerp.Api.WebServices.SocialAPI;
using Exerp.Api.WebServices.SubscriptionAPI;
using PureGym.Core.Interfaces;

namespace Exerp.Api
{
    public class DependencyConfiguration : Module
    {
        private readonly IExerpConfiguration _config;

        public DependencyConfiguration(IExerpConfiguration config)
        {
            _config = config;
        }

        protected override void Load(ContainerBuilder builder)
        {
   
            //WebServices
            builder.Register(c => new PersonAPIService().WithConfig(_config)).As<IPersonApi>();
            builder.Register(c => new SelfServiceAPIService().WithConfig(_config)).As<ISelfServiceApi>();
            builder.Register(c => new SubscriptionAPIService().WithConfig(_config)).As<ISubscriptionApi>();
            builder.Register(c => new BookingAPIService().WithConfig(_config)).As<IBookingApi>();
            builder.Register(c => new SocialAPIService().WithConfig(_config)).As<ISocialApi>();

            //Services
            builder.Register(c => new BookingService(c.Resolve<IBookingApi>())).As<IBookingService>().SingleInstance();
            builder.Register(c => new PersonService(c.Resolve<ISelfServiceApi>(), c.Resolve<IPersonApi>(), c.Resolve<ICentreService>(), c.Resolve<ISubscriptionApi>(), c.Resolve<ISocialApi>(), c.Resolve<IBookingApi>(), _config)).As<IPersonService>().SingleInstance();
            builder.Register(c => new ClipCardPurchaseService(c.Resolve<ISubscriptionApi>(), c.Resolve<ICentreService>())).As<IClipCardPurchaseService>().SingleInstance();

            //Cached Services
            builder.Register(c => new CentreServiceCached(new CentreService(c.Resolve<IPersonApi>(), _config), c.Resolve<ICacheProvider>())).As<ICentreService>().SingleInstance();

            MappingConfiguration.ConfigureMappings();
        }
    }
}
