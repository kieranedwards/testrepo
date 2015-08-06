using System.IO;
using System.Net;
using System.Web;
using System.Web.Routing;

namespace PureRide.WebUI.Routes
{
    internal sealed class StaticFileRouteHandler : IRouteHandler
    {
        public IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            return new StaticFileHttpHandler(string.Concat("~/Content", requestContext.HttpContext.Request.FilePath));
        }
    }

    public class StaticFileHttpHandler : IHttpHandler
    {
        private const string ImageContentType = "image/png";
        private const string IconContentType = "image/x-icon";

        private readonly string _filename;

        public StaticFileHttpHandler(string filename)
        {
            _filename = filename;
        }

        public void ProcessRequest(HttpContext context)
        {
            context.Response.Clear();

            string filepath = context.Server.MapPath(this._filename);

            if (!File.Exists(filepath))
                context.Response.StatusCode = (int)HttpStatusCode.NotFound;
            else
            {
                context.Response.ContentType = _filename.EndsWith(".ico") ? IconContentType : ImageContentType;
                context.Response.WriteFile(filepath);
            }
            
            context.Response.End();
        }

        public bool IsReusable
        {
            get { return false; }
        }
    }
}