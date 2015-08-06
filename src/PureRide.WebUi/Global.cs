using System;
using System.Diagnostics;
using System.Net;
using System.Web;
using System.Web.Optimization;
using PureRide.WebUI.Routes;
using Umbraco.Web;

namespace PureRide.WebUI
{
    public class Global : UmbracoApplication
    {
        protected override void OnApplicationStarted(object sender, EventArgs e)
        {
            DependencyConfiguration.RegisterDependencies();
            RouteConfiguration.CustomRouteRegistration();
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            Trace.AutoFlush = true;
        }

        protected void Application_BeginRequest()
        {
            RedirectToSslIfRequired();
        }

        private void RedirectToSslIfRequired()
        {
            if (!Request.IsLocal && !Request.IsSecureConnection)
            {
                string redirectUrl = Request.Url.ToString().Replace("http:", "https:");
                Response.Redirect(redirectUrl, false);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }
        }

        protected override void OnApplicationError(object sender, EventArgs e)
        {
            var error = Server.GetLastError();
            if (error is UnauthorizedAccessException)
            {
                error = new UnauthorizedAccessException(string.Concat(GetPublicIp(),"  was unable to access Exerp API due to their firewall settings."), error);
            }
            Trace.TraceError(error.Message);
        }


        private static string GetPublicIp()
        {
            return new WebClient().DownloadString("https://api.ipify.org");
        }

    }
}
