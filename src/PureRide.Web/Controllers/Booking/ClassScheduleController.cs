using System.Web.Mvc;
using PureRide.Web.ApplicationServices.Booking;
using PureRide.Web.Helpers;
using Umbraco.Web.Models;
using Umbraco.Web.Mvc;

namespace PureRide.Web.Controllers.Booking
{
    public class ClassScheduleController : RenderMvcController
    {
        private readonly IClassScheduleViewModelBuilder _classScheduleViewModelBuilder;
        private readonly IBookingAvailabilityMessageBuilder _bookingAvailabilityMessageBuilder;

        public ClassScheduleController(IClassScheduleViewModelBuilder classScheduleViewModelBuilder, IBookingAvailabilityMessageBuilder bookingAvailabilityMessageBuilder)
        {
            _classScheduleViewModelBuilder = classScheduleViewModelBuilder;
            _bookingAvailabilityMessageBuilder = bookingAvailabilityMessageBuilder;
        }

        public ActionResult ClassSchedule(RenderModel model, string location)
        {
            var classScheduleModel = _classScheduleViewModelBuilder.BuildModel(location);

            if(classScheduleModel == null)
                return new HttpNotFoundResult();

            classScheduleModel.Messages = _bookingAvailabilityMessageBuilder.BuildMessages();
            return Index(classScheduleModel.AsRenderModel());
        }
        
      
    }
}