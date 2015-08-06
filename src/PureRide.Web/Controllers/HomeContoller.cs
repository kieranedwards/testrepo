using System.Web.Mvc;
using PureRide.Web.ApplicationServices;
using PureRide.Web.Configuration;
using PureRide.Web.ViewModels;
using Umbraco.Web.Mvc;

namespace PureRide.Web.Controllers
{
    public class HomeController : SurfaceController
    {
        private readonly ISiteSettings _siteSettings;
        private readonly ICookieNotificationService _cookieNotificationService;

        public HomeController(ISiteSettings siteSettings, ICookieNotificationService cookieNotificationService)
        {
            _siteSettings = siteSettings;
            _cookieNotificationService = cookieNotificationService;
        }

        [ChildActionOnly]
        public ActionResult CookieNotification()
        {
            if (_cookieNotificationService.UserHasViewedCookie())
                return new EmptyResult();
            
            
            _cookieNotificationService.SetUserCookie();
            return View();    
        }

        [ChildActionOnly]
        public ActionResult SiteEnvironmentNotification()
        {
            if(_siteSettings.SiteEnvironment == Environment.Live)
                return new EmptyResult();

            return View(new SiteEnvironmentModel() { Version = _siteSettings.SiteVersion, Environment = _siteSettings.SiteEnvironment });
        }

    }
}
