using System.Web.Http;
using System.Web.Mvc;
using PureRide.Web.ApplicationServices;
using PureRide.Web.ApplicationServices.Booking;
using PureRide.Web.ViewModels.Booking;
using Umbraco.Web.Mvc;

namespace PureRide.Web.Controllers.Booking
{
    public class ClassScheduleApiController : ApiController
    {
        private readonly IClassScheduleViewModelBuilder _classScheduleViewModelBuilder;

        public ClassScheduleApiController(IClassScheduleViewModelBuilder classScheduleViewModelBuilder)
        {
            _classScheduleViewModelBuilder = classScheduleViewModelBuilder;
        }

        [System.Web.Http.HttpGet]
        public ActionResult ClassSchedule(string location)
        {
            var model = _classScheduleViewModelBuilder.BuildModel(location);
            return new JsonNetResult() { Data = model };
        }
    }


}
