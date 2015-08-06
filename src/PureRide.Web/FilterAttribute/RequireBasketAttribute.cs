using System.Web;
using System.Web.Mvc;
using PureRide.Web.ApplicationServices.Credits;

namespace PureRide.Web.FilterAttribute
{
    public class RequireBasketAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (HttpContext.Current.Request.Cookies[CreditPackBasketService.PendingPurchaseCookieName] == null)
            {
                if (filterContext.IsChildAction)
                    filterContext.ParentActionViewContext.HttpContext.Response.Redirect("/credits");
                else
                    filterContext.Result = new RedirectToRouteResult("/credits",null);
            }
        }
    }
}