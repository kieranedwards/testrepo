using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PureRide.Web.Providers
{
    using System.Web;

    public interface IHttpContextProvider
    {
        HttpContextBase Current { get; }
    }

    internal class HttpContextProvider : IHttpContextProvider
    {
        public HttpContextBase Current
        {
            get
            {
                if (HttpContext.Current == null)
                    return null;

                return new HttpContextWrapper(HttpContext.Current);
            }
        }
    }
}
