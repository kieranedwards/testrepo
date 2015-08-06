using System;
using PureRide.Web.ViewModels.Booking;

namespace PureRide.Web.ApplicationServices.Booking.ClassAvailabilityStatus
{
    public class BookableStatus : IClassAvailabilityStatus
    {
        public BookingAvailability AvailabilityStatus {
            get { return BookingAvailability.Bookable;}
        }

        public bool IsValid(DateTime startTime)
        {
            return true;
        }
    }
}