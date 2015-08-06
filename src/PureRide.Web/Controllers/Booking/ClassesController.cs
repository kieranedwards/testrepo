using System.Web.Mvc;
using PureRide.Web.ApplicationServices;
using PureRide.Web.ApplicationServices.Location;
using PureRide.Web.Configuration;
using PureRide.Web.Helpers;
using Umbraco.Web.Models;
using Umbraco.Web.Mvc;

namespace PureRide.Web.Controllers.Booking
{
    public class ClassesController : RenderMvcController
    {
        private readonly ILocationViewModelBuilder _locationViewModelBuilder;
        private readonly ISiteSettings _settings;

        public ClassesController(ILocationViewModelBuilder locationViewModelBuilder,ISiteSettings settings)
        {
            _locationViewModelBuilder = locationViewModelBuilder;
            _settings = settings;
        }

        public override ActionResult Index(RenderModel model)
        {
            if (!string.IsNullOrWhiteSpace(_settings.SingleLocationUrl))
                return Redirect(string.Concat(_settings.SingleLocationUrl, "classes/"));

            var locations = _locationViewModelBuilder.BuildModelForAll();
            return base.Index(locations.AsRenderModel());
        }
    }
}
