using System;
using System.Configuration;
using System.Net;
using Exerp.Api.Interfaces.Configuration;
using PureRide.Data.Interfaces.Configuration;
using SagePay.Interfaces;

namespace PureRide.Web.Configuration
{

    /// <summary>
    /// Core settings not changed at run time from the web.config file
    /// </summary>
    /// <remarks>
    /// To add dynamic settings see http://www.cooking-on-gas.com/articles/2014/march/28/adding-global-values-to-an-umbraco-website/ 
    /// Umbraco also has a GlobalSettings class
    /// </remarks>
    public class WebSiteSettings : ISiteSettings, IScheduleSettings, ISagePageSettings, IExerpConfiguration, IDatabaseConnectionSettings
    {
        public static string GetCdnRoot()
        {
            return GetSetting("cdnRoot");
        }

        public static string GetSetting(string settingName)
        {
            return Convert.ToString(ConfigurationManager.AppSettings[settingName]);
        }

        public static T GetSetting<T>(string settingName)
        {
            object value = ConfigurationManager.AppSettings[settingName];
            return (T) Convert.ChangeType(value, typeof (T));
        }

        public string SagePayEndPoint
        {
            get { return GetSetting("SagePay.EndPoint"); }
        }

        public string SagePayNotificationUrl
        {
            get { return GetSetting("SagePay.NotificationUrl"); }
        }

        public string SagePayReturnUrl
        {
            get { return GetSetting("SagePay.ReturnUrl"); }
        }

        public string SagePayVendorName
        {
            get { return GetSetting("SagePay.VendorName"); }
        }

        public string ExerpApiBaseUrl
        {
            get { return GetSetting("Exerp.ApiBaseUrl"); }
        }

        public NetworkCredential ExerpApiCredentials
        {
            get { return new NetworkCredential(GetSetting("Exerp.ApiUserName"), GetSetting("Exerp.ApiPassword")); }
        }

        public int ExerpPureRideScopeId
        {
            get { return GetSetting<int>("Exerp.PureRideScopeId"); }
        }

        public int ExerpPureRidePrimaryCentreId
        {
            get { return GetSetting<int>("Exerp.PureRidePrimaryCentreId"); }
        }
         
        public string PureRideConnectionString {
            get { return ConfigurationManager.ConnectionStrings["PureRideDatabase"].ConnectionString; }
        }

        public string BookingConnectionString
        {
            get { return ConfigurationManager.ConnectionStrings["BookingDatabase"].ConnectionString; }
        }

        public string SiteVersion
        {
            get { return GetSetting("Site.Version"); }
        }

        public Environment SiteEnvironment
        {
            get
            {
                Environment mode;
                Enum.TryParse(GetSetting("Site.Environment"), out mode);
                return mode;
            }
        }
      
        public string SingleLocationUrl {
            get
            {
                return GetSetting("Site.SingleLocationUrl");
            }
        }

        public int MaxVisibleDays{get{return 14;}}
        public int MaxBookableDays {get { return 8; }}
        public int MinBookableMinutesBeforeStart {get { return 30; }}
    }
}
