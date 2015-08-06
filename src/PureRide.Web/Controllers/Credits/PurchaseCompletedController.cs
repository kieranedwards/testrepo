using System.Web.Mvc;
using PureRide.Web.ApplicationServices.Credits;
using Umbraco.Web.Models;
using Umbraco.Web.Mvc;

namespace PureRide.Web.Controllers.Credits
{
    [Authorize]
    public class PurchaseCompletedController : RenderMvcController
    {
        private readonly ICreditPackBasketService _creditPackBasketService;

        public PurchaseCompletedController(ICreditPackBasketService creditPackBasketService)
        {
            _creditPackBasketService = creditPackBasketService;
        }

        public override ActionResult Index(RenderModel model)
        {
            
            _creditPackBasketService.ClearSelectedClipCard();
            return base.Index(model);
        }
    }
}
