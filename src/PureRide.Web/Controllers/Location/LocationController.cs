using System.Web.Mvc;
using PureRide.Web.ApplicationServices;
using PureRide.Web.ApplicationServices.Location;
using PureRide.Web.Configuration;
using PureRide.Web.Helpers;
using Umbraco.Web.Models;
using Umbraco.Web.Mvc;

namespace PureRide.Web.Controllers
{
    public class LocationPageController : RenderMvcController 
    {
        private readonly ILocationViewModelBuilder _locationViewModelBuilder;
        private readonly ISiteSettings _settings;

        public LocationPageController(ILocationViewModelBuilder locationViewModelBuilder,
            ISiteSettings settings)
        {
            _locationViewModelBuilder = locationViewModelBuilder;
            _settings = settings;
        }

        public override ActionResult Index(RenderModel model)
        {
            if (!string.IsNullOrWhiteSpace(_settings.SingleLocationUrl))
                return Redirect(_settings.SingleLocationUrl);
            
            var locationModel = _locationViewModelBuilder.BuildModelForAll();
            return base.Index(locationModel.AsRenderModel());
        }

    }
}
