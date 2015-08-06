using System.Web.Http;
using System.Web.Mvc;
using PureRide.Web.ApplicationServices;
using PureRide.Web.ApplicationServices.Location;
using Umbraco.Web.Mvc;

namespace PureRide.Web.Controllers
{
    public class LocationApiController : ApiController
    {
        private readonly ILocationViewModelBuilder _locationViewModelBuilder;

        public LocationApiController(ILocationViewModelBuilder locationViewModelBuilder)
        {
            _locationViewModelBuilder = locationViewModelBuilder;
        }

        [System.Web.Http.HttpGet]
        public ActionResult Locations(string region)
        {
            var model = _locationViewModelBuilder.BuildModelForAll();
            return new JsonNetResult() { Data = model };
        }
    }
}
