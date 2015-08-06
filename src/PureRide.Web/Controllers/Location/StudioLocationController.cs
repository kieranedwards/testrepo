using System.Net;
using System.Web.Mvc;
using PureRide.Web.ApplicationServices;
using PureRide.Web.ApplicationServices.Location;
using PureRide.Web.Helpers;
using Umbraco.Web.Models;
using Umbraco.Web.Mvc;

namespace PureRide.Web.Controllers.Location
{
    public class StudioLocationController : RenderMvcController
    {
        private readonly ILocationViewModelBuilder _locationViewModelBuilder;

        public StudioLocationController(ILocationViewModelBuilder locationViewModelBuilder)
        {
            _locationViewModelBuilder = locationViewModelBuilder;
        }

        public ActionResult StudioLocation(RenderModel model, string location)
        {
            var locationModel = _locationViewModelBuilder.BuildModelForLocation(location.FromStudioSlug());

            if(locationModel==null)
                return HttpNotFound();

            return Index(locationModel.AsRenderModel());
        }

    }
}
