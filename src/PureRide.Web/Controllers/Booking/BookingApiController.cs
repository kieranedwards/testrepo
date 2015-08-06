 
using System;
using System.Web.Http;
using System.Web.Mvc;
using Exerp.Api.DataTransfer;
using PureRide.Web.ApplicationServices.Booking;
using PureRide.Web.ViewModels.Booking;
using Umbraco.Web.Mvc;

namespace PureRide.Web.Controllers.Booking
{
    public class BookingApiController : ApiController
    {
        private readonly IBookingManagerService _bookingManagerService;
        private readonly IBookingResultUrlBuilder _bookingResultUrlBuilder;

        public BookingApiController(IBookingManagerService bookingManagerService, IBookingResultUrlBuilder bookingResultUrlBuilder)
        {
            _bookingManagerService = bookingManagerService;
            _bookingResultUrlBuilder = bookingResultUrlBuilder;
        }

        [System.Web.Http.HttpPost]
        public ActionResult IsBooked(BookingQueryModel model)
        {
            return new JsonNetResult() { Data = _bookingManagerService.PersonIsBookedAnyClass(new Identity(model.PersonId), DateTime.MinValue, DateTime.MaxValue) };
        }

        [System.Web.Http.HttpPost]
        public ActionResult PlaceBooking(BookingDetailsModel model)
        {
            _bookingManagerService.BookClass(model);
            return new JsonNetResult() { Data = new { Url = _bookingResultUrlBuilder.Build() } };
        }

        [System.Web.Http.HttpPost]
        public ActionResult CancelBooking(BookingCancelModel model)
        {
            _bookingManagerService.CancelClass(model);
            return new JsonNetResult() { Data = new { Url = _bookingResultUrlBuilder.Build() } };
        }
    }

}
