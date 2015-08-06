using System.Web.Mvc;
using PureRide.Web.ApplicationServices;
using PureRide.Web.ApplicationServices.Credits;
using PureRide.Web.FilterAttribute;
using Umbraco.Web.Models;
using Umbraco.Web.Mvc;

namespace PureRide.Web.Controllers.Credits
{
    [Authorize, RequireBasket]
    public class PaymentController : RenderMvcController
    {
        private readonly IPaymentViewModelBuilder _paymentViewModelBuilder;
        
        public PaymentController(IPaymentViewModelBuilder paymentViewModelBuilder)
        {
            _paymentViewModelBuilder = paymentViewModelBuilder;
        }

        public ActionResult Payment(RenderModel model)
        {
            ViewBag.Url = _paymentViewModelBuilder.BuildModel().Url;
            return Index(model);
        }
 
    }
}