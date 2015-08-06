using System;
using PureRide.Web.Configuration;
using PureRide.Web.ViewModels.Booking;

namespace PureRide.Web.ApplicationServices.Booking.ClassAvailabilityStatus
{
    public class StartingSoonStatus : IClassAvailabilityStatus
    {
        private readonly IScheduleSettings _scheduleSettings;

        public StartingSoonStatus(IScheduleSettings scheduleSettings)
        {
            _scheduleSettings = scheduleSettings;
        }

        public BookingAvailability AvailabilityStatus
        {
            get { return BookingAvailability.StartingSoon; }
        }

        public bool IsValid(DateTime startTime)
        {
            var minutesUntilStart = startTime.Subtract(DateTime.Now).TotalMinutes;
            return minutesUntilStart > 0 && minutesUntilStart <= _scheduleSettings.MinBookableMinutesBeforeStart;
        }
    }
}