using System;
using System.Web;
using PureRide.Web.Providers;

namespace PureRide.Web.ApplicationServices
{
    public class CookieNotificationService : ICookieNotificationService
    {
        private readonly IHttpContextProvider _httpContextProvider;
        internal const string CookieName = "CookieNotification";

        public CookieNotificationService(IHttpContextProvider httpContextProvider)
        {
            _httpContextProvider = httpContextProvider;
        }

        public bool UserHasViewedCookie()
        {
            return (_httpContextProvider.Current.Request.Cookies[CookieName] != null);
        }

        public void SetUserCookie()
        {
            var cookie = new HttpCookie(CookieName)
            {
                Expires = DateTime.Now.AddDays(90),
                HttpOnly = true,
                Shareable = false,
                Secure = _httpContextProvider.Current.Request.IsSecureConnection
            };

            _httpContextProvider.Current.Response.Cookies.Add(cookie);
        }
    }

    public interface ICookieNotificationService
    {
        bool UserHasViewedCookie();
        void SetUserCookie();
    }
}
