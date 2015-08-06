using System;
using Exerp.Api.DataTransfer;
using PureRide.Web.ViewModels.Booking;

namespace PureRide.Web.ApplicationServices.Booking.ClassAvailabilityStatus
{
    public interface IClassAvailabilityStatus
    {
        BookingAvailability AvailabilityStatus { get; }
        bool IsValid(DateTime startTime);
    }
}