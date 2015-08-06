using System.Web;
using Umbraco.Web;

namespace PureRide.Web.Providers
{
    public class UmbracoHelperProvider : IUmbracoHelperProvider
    {
        public IUmbracoHelper GetHelper()
        {
            return new UmbracoHelper();
        }
    }

    internal class UmbracoHelper : IUmbracoHelper
    {
        private readonly Umbraco.Web.UmbracoHelper _umbracoHelper;

        internal UmbracoHelper()
        {
            _umbracoHelper = new Umbraco.Web.UmbracoHelper(UmbracoContext.Current);
        }

        public IHtmlString GetFieldRecursive(string fieldAlias)
        {
            return _umbracoHelper.Field(fieldAlias, recursive: true);
        }
    }

    public interface IUmbracoHelperProvider
    {
        IUmbracoHelper GetHelper();
    }

    public interface IUmbracoHelper
    {
        IHtmlString GetFieldRecursive(string fieldAlias);
    }


}
