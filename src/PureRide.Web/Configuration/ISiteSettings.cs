namespace PureRide.Web.Configuration
{
    public interface ISiteSettings
    {
        string SiteVersion { get; }
        Environment SiteEnvironment { get; }
        string SingleLocationUrl { get; }
    }
}