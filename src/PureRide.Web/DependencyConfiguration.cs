using System.Collections.Generic;
using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using Exerp.Api.Interfaces.Services;
using PureGym.Core.Interfaces;
using PureRide.Data.Repositories;
using PureRide.Web.ApplicationServices;
using PureRide.Web.ApplicationServices.Account;
using PureRide.Web.ApplicationServices.Booking;
using PureRide.Web.ApplicationServices.Booking.ClassAvailabilityStatus;
using PureRide.Web.ApplicationServices.Credits;
using PureRide.Web.ApplicationServices.Location;
using PureRide.Web.Configuration;
using PureRide.Web.Controllers.Account;
using PureRide.Web.Providers;
using SagePay.Interfaces;

namespace PureRide.Web
{
    public class DependencyConfiguration : Module
    {
      protected override void Load(ContainerBuilder builder)
      {
          var settings = new WebSiteSettings();
          builder.Register(c => settings).As<ISagePageSettings>();
          builder.Register(c => settings).As<IScheduleSettings>();

          //Providers, Helpers and Mappers
          builder.Register(c => new HttpContextProvider()).As<IHttpContextProvider>();
          builder.Register(c => new CreditPackModelAdapter()).As<ICreditPackModelAdapter>();
          builder.Register(c => new UmbracoHelperProvider()).As<IUmbracoHelperProvider>();
          builder.Register(c => new PaymentListenerUrlProvider(c.Resolve<IHttpContextProvider>(), settings)).As<IPaymentListenerUrlProvider>();
           
          //Status
          builder.Register(c => new BookableStatus()).As<IClassAvailabilityStatus>();
          builder.Register(c => new AlreadyBookedStatus()).As<IClassAvailabilityStatus>();
          builder.Register(c => new FutureClassStatus(settings)).As<IClassAvailabilityStatus>();
          builder.Register(c => new StartingSoonStatus(settings)).As<IClassAvailabilityStatus>();
          builder.Register(c => new PastClassStatus()).As<IClassAvailabilityStatus>();
          builder.Register(c => new WaitingListStatus()).As<IClassAvailabilityStatus>();

          //Application Services
          builder.Register(c => new AuthenticationService(c.Resolve<IHttpContextProvider>(),c.Resolve<IPersonService>())).As<IAuthenticationService>();
          builder.Register(c => new PersonRegistrationService(c.Resolve<IPersonService>(),c.Resolve<IAuthenticationService>())).As<IPersonRegistrationService>();
          builder.Register(c => new PasswordManagementService(c.Resolve<IPersonService>(),c.Resolve<IAuthenticationService>())).As<IPasswordManagementService>();
          builder.Register(c => new CreditPackBasketService(c.Resolve<IHttpContextProvider>(),c.Resolve<IAuthenticationService>(),c.Resolve<IClipCardPurchaseService>(), c.Resolve<IClipCardRepository>())).As<ICreditPackBasketService>();
          builder.Register(c => new CookieNotificationService(c.Resolve<IHttpContextProvider>())).As<ICookieNotificationService>();

          builder.Register(c => new ScheduledClassModelAdapter(c.Resolve<IEnumerable<IClassAvailabilityStatus>>())).As<IScheduledClassModelAdapter>();
          builder.Register(c => new BookingResultUrlBuilder()).As<IBookingResultUrlBuilder>();
          builder.Register(c => new CreditPackPurchaseService(c.Resolve<IPaymentListenerUrlProvider>(), c.Resolve<IClipCardRepository>(), c.Resolve<IClipCardPurchaseService>(), c.Resolve<ICreditPackBasketService>())).As<ICreditPackPurchaseService>();
           
          builder.Register(c => new BookingManagerService(c.Resolve<IBookingService>(), c.Resolve<IPersonService>(), c.Resolve<IAuthenticationService>())).As<IBookingManagerService>();
           

          builder.Register(c => new ClassScheduleViewModelBuilder(c.Resolve<ICentreService>(), c.Resolve<IBookingService>(), c.Resolve<IScheduleSettings>(), c.Resolve<IScheduledClassModelAdapter>())).As<IClassScheduleViewModelBuilder>();
          builder.Register(c => new BookingAvailabilityMessageBuilder(c.Resolve<IUmbracoHelperProvider>())).As<IBookingAvailabilityMessageBuilder>();
          builder.Register(c => new CreditBalanceService(c.Resolve<IAuthenticationService>(), c.Resolve<IPersonService>())).As<ICreditBalanceService>();
          
          builder.Register(c => settings).As<ISiteSettings>();
          
          //View Model Builders
          builder.Register(c => new CreditHistoryViewModelBuilder(c.Resolve<IPersonService>(), c.Resolve<IAuthenticationService>())).As<ICreditHistoryViewModelBuilder>();
          builder.Register(c => new LocationViewModelBuilder(c.Resolve<ICentreService>())).As<ILocationViewModelBuilder>();
          builder.Register(c => new AccountDashboardViewModelBuilder(c.Resolve<IAuthenticationService>(), c.Resolve<IPersonService>(),c.Resolve<ICreditBalanceService>())).As<IAccountDashboardViewModelBuilder>();
          builder.Register(c => new BillingAddressViewModelBuilder(c.Resolve<IAuthenticationService>(), c.Resolve<IPersonService>())).As<IBillingAddressViewModelBuilder>();
          builder.Register(c => new SeatSelectionModelBuilder(c.Resolve<ICentreService>(), 
              c.Resolve<ICreditBalanceService>(),
              c.Resolve<IBookingService>(),
              c.Resolve<IScheduledClassModelAdapter>(),
              c.Resolve<IBookingAvailabilityMessageBuilder>(),
              c.Resolve<IPersonService>(),
              c.Resolve<IAuthenticationService>()
              )).As<ISeatSelectionModelBuilder>();

          builder.Register(c => new PaymentViewModelBuilder(c.Resolve<IAuthenticationService>(),
              c.Resolve<IPersonService>(),
              c.Resolve<ITransactionClient>(),
              c.Resolve<ITransactionRequestBuilderFactory>(),
              c.Resolve<ISagePageSettings>(),
              c.Resolve<ICreditPackBasketService>(),
              c.Resolve<ICreditPackPurchaseService>()
              )).As<IPaymentViewModelBuilder>();

          builder.Register(c => new CreditPacksViewModelBuilder(
              c.Resolve<IClipCardPurchaseService>(),
              c.Resolve<IAuthenticationService>(), 
              c.Resolve<ICentreService>(),
              c.Resolve<ICreditPackModelAdapter>()
              )).As<ICreditPacksViewModelBuilder>();

          builder.Register(c => new UpdatePersonDetailsModelBuilder(
              c.Resolve<IPersonService>(),
              c.Resolve<IAuthenticationService>()
              )).As<IUpdateDetailsModelBuilder>();

          builder.Register(c => new HttpRuntimeCacheProvider()).As<ICacheProvider>();
          
          builder.RegisterControllers(typeof(AccountController).Assembly);
          builder.RegisterApiControllers(typeof(AccountController).Assembly);

          MappingConfiguration.ConfigureMappings();
      }
    }
}
