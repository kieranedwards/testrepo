using System.Web.Mvc;
using PureRide.Web.ApplicationServices.Credits;
using PureRide.Web.Helpers;
using PureRide.Web.ViewModels.Credits;
using Umbraco.Web.Models;
using Umbraco.Web.Mvc;

namespace PureRide.Web.Controllers.Credits
{
    public class CreditPacksController : RenderMvcController
    {
        private readonly ICreditPacksViewModelBuilder _creditPacksViewModelBuilder;

        public CreditPacksController(ICreditPacksViewModelBuilder creditPacksViewModelBuilder)
        {
            _creditPacksViewModelBuilder = creditPacksViewModelBuilder;
        }

        public ActionResult CreditPacks(RenderModel model, string location,string code)
        {
            var locationModel = _creditPacksViewModelBuilder.BuildModel(location, code);

            if (locationModel == null)
                return new HttpNotFoundResult();

            return Index(locationModel.AsRenderModel());
        }
    }
}