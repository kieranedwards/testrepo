using System;
using PureRide.Web.ViewModels.Booking;

namespace PureRide.Web.ApplicationServices.Booking.ClassAvailabilityStatus
{
    public class PastClassStatus : IClassAvailabilityStatus
    {
        public BookingAvailability AvailabilityStatus
        {
            get { return BookingAvailability.PastClass; }
        }

        public bool IsValid(DateTime startTime)
        {
            return startTime < DateTime.Now;
        }
    }
}