using System;
using Exerp.Api.Interfaces.Services;
using PureRide.Web.ViewModels.Booking;

namespace PureRide.Web.ApplicationServices.Booking.ClassAvailabilityStatus
{
    public class AlreadyBookedStatus : IClassAvailabilityStatus
    {
 
        public BookingAvailability AvailabilityStatus
        {
            get { return BookingAvailability.StartingSoon; }
        }

        public bool IsValid(DateTime startTime)
        {
            return false;
        }
    }
}