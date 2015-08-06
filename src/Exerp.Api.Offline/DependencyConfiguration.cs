using Autofac;
using Exerp.Api.Interfaces.Services;
using Exerp.Api.Offline.SampleData;
using Exerp.Api.Offline.Services;

namespace Exerp.Api.Offline
{
    public class DependencyConfiguration : Module
    {
  
        protected override void Load(ContainerBuilder builder)
        {
             
            builder.Register(c => new SampleDataProvider()).As<ISampleDataProvider>();
            builder.Register(c => new BookingService(c.Resolve<ISampleDataProvider>())).As<IBookingService>();
            builder.Register(c => new CentreService(c.Resolve<ISampleDataProvider>())).As<ICentreService>();
            builder.Register(c => new PersonService(c.Resolve<ISampleDataProvider>())).As<IPersonService>();
            builder.Register(c => new ClipPackPurchaseService(c.Resolve<ISampleDataProvider>(),c.Resolve<ICentreService>())).As<IClipCardPurchaseService>();
        }
    }
}
