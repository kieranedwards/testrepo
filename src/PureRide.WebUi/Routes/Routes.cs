using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using PureRide.Web;
using Umbraco.Web;

namespace PureRide.WebUI.Routes
{
    public class RouteConfiguration
    {
        public static void CustomRouteRegistration()
        {

            RouteTable.Routes.Add(new Route("error-test", new TestErrorRouteHandler()));

            //Icons
            RouteTable.Routes.Add(new Route("favicon.ico", new StaticFileRouteHandler()));
            RouteTable.Routes.Add(new Route("apple-touch-icon{size}.png", new StaticFileRouteHandler()));
            RouteTable.Routes.Add(new Route("apple-touch-icon.png", new StaticFileRouteHandler()));
            RouteTable.Routes.Add(new Route("favicon-{size}.png", new StaticFileRouteHandler()));

            //Umbraco
            RouteTable.Routes.MapUmbracoRoute("studio-locations", "{location}-studio", new { controller = "StudioLocation", action = "StudioLocation" }, new StudioLocationRouteHandler());
            RouteTable.Routes.MapUmbracoRoute("class-schedule", "{location}-studio/classes", new { controller = "ClassSchedule", action = "ClassSchedule" }, new PublishedPageRouteHandler(WellKnownPage.ClassSchedulePublishedPage));
            RouteTable.Routes.MapUmbracoRoute("credit-packs", "{location}-studio/credits", new { controller = "CreditPacks", action = "CreditPacks" }, new PublishedPageRouteHandler(WellKnownPage.CreditPacksPublishedPage));
            RouteTable.Routes.MapUmbracoRoute("select-seat", "{location}-studio/classes/{classname}-{classid}", new { controller = "SeatSelection", action = "SeatSelection" }, new PublishedPageRouteHandler(WellKnownPage.SeatSelection));

            //API for React
            RouteTable.Routes.MapHttpRoute(null, "api/locations/{region}", new { controller = "LocationApi", action = "Locations"});
            RouteTable.Routes.MapHttpRoute(null, "api/credits/select", new { controller = "CreditPacksApi", action = "Select" });
            RouteTable.Routes.MapHttpRoute(null, "api/credits/payment-notification", new { controller = "PaymentListenerApi", action="Index" });
            RouteTable.Routes.MapHttpRoute(null, "api/credits/{location}-studio", new { controller = "CreditPacksApi", action = "CreditPacks" });
            RouteTable.Routes.MapHttpRoute(null, "api/classes/{location}-studio", new { controller = "ClassScheduleApi", action = "ClassSchedule" });
            RouteTable.Routes.MapHttpRoute(null, "api/account/{action}", new { controller = "AccountApi"});
            RouteTable.Routes.MapHttpRoute(null, "api/booking/{action}", new { controller = "BookingApi"});
            RouteTable.Routes.MapHttpRoute(null, "api/credits/{action}", new { controller = "CreditPacksApi" });
            
            //Clear XML so we only return JSON API results
            GlobalConfiguration.Configuration.Formatters.XmlFormatter.SupportedMediaTypes.Clear();
        }

    }
}