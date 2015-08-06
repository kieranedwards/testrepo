using System;
using PureRide.Web.ViewModels.Booking;

namespace PureRide.Web.ApplicationServices.Booking.ClassAvailabilityStatus
{
    public class WaitingListStatus : IClassAvailabilityStatus
    {
        public BookingAvailability AvailabilityStatus
        {
            get { return BookingAvailability.WaitingList; }
        }

        public bool IsValid(DateTime startTime)
        {
            return false;
        }
    }
}