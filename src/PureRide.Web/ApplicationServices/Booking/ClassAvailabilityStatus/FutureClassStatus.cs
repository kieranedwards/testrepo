using System;
using PureRide.Web.Configuration;
using PureRide.Web.ViewModels.Booking;

namespace PureRide.Web.ApplicationServices.Booking.ClassAvailabilityStatus
{
    public class FutureClassStatus : IClassAvailabilityStatus
    {
        private readonly IScheduleSettings _scheduleSettings;

        public FutureClassStatus(IScheduleSettings scheduleSettings)
        {
            _scheduleSettings = scheduleSettings;
        }

        public BookingAvailability AvailabilityStatus
        {
            get { return BookingAvailability.FutureClass; }
        }

        public bool IsValid(DateTime startTime)
        {
            var daysUntilStart = startTime.Subtract(DateTime.Now).TotalDays;
            return daysUntilStart > _scheduleSettings.MaxBookableDays;
        }
    }
}