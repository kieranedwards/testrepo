using System.Web.Mvc;
using PureRide.Web.ApplicationServices.Credits;
using PureRide.Web.FilterAttribute;
using PureRide.Web.Helpers;
using PureRide.Web.ViewModels.Credits;
using Umbraco.Web.Mvc;

namespace PureRide.Web.Controllers.Credits
{
    [Authorize, RequireBasket]
    public class BillingAddressController : SurfaceController
    {
        private readonly IBillingAddressViewModelBuilder _billingAddressViewModelBuilder;

        public BillingAddressController(IBillingAddressViewModelBuilder billingAddressViewModelBuilder)
        {
            _billingAddressViewModelBuilder = billingAddressViewModelBuilder;
        }

        [ChildActionOnly]
        public ActionResult BillingAddressForm()
        {
            var model = _billingAddressViewModelBuilder.BuildModel();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult FormSubmit(BillingAddressModel model)
        {
             if (!ModelState.IsValid) 
                return CurrentUmbracoPage().WithModel(TempData,model);

            _billingAddressViewModelBuilder.UpdateFromModel(model);

             return RedirectToUmbracoPage((int)WellKnownPage.PaymentPublishedPage);
        }

    }
}