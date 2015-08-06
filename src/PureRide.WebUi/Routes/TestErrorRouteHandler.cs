using System.Web;
using System.Web.Routing;

namespace PureRide.WebUI.Routes
{
    public class TestErrorRouteHandler : IRouteHandler
    {
        public IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            throw new System.SystemException("Test Exception");
        }
    }
}