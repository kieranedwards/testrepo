using System.Collections.Generic;
using System.Linq;
using Exerp.Api.DataTransfer;
using PureRide.Web.ApplicationServices.Booking.ClassAvailabilityStatus;
using PureRide.Web.ViewModels.Booking;

namespace PureRide.Web.ApplicationServices.Booking
{
    public interface IScheduledClassModelAdapter
    {
        ScheduledClassModel Create(ScheduledClass source);
    }

    public class ScheduledClassModelAdapter : IScheduledClassModelAdapter
    {
        private readonly IEnumerable<IClassAvailabilityStatus> _bookingMessages;

        public ScheduledClassModelAdapter(IEnumerable<IClassAvailabilityStatus> bookingMessages)
        {
            _bookingMessages = bookingMessages;
        }

        public ScheduledClassModel Create(ScheduledClass source)
        {
            var availabilityStatus = GetBookingAvailabilityStatus(source);

            return new ScheduledClassModel()
            {
                ClassId = source.BookingId.ToString(),
                InstructorName = source.InstructorName,
                ClassName = source.Name,
                RoomName = source.RoomName,
                IsWaitingList = source.ClassCapacity <= (source.BookedCount + source.WaitingListCount),
                ClassCapacity = source.ClassCapacity,
                IsNearWaitingList = (source.ClassCapacity - (source.BookedCount + source.WaitingListCount) <=5),
                BookedCount = source.BookedCount,
                ClassTime = BuildClassTime(source),
                ClassDate = source.StartTime.ToShortDateString(),
                IsBookingUnavailable = IsBookingUnavailable(availabilityStatus),
                AvailabilityStatus = availabilityStatus
            };
        }

        private BookingAvailability GetBookingAvailabilityStatus(ScheduledClass scheduledClass)
        {

            var matches = _bookingMessages
                .Where(a => a.IsValid(scheduledClass.StartTime)).ToArray();

            return matches
                .OrderBy(a => (int)a.AvailabilityStatus)
                .First()
                .AvailabilityStatus;
        }

        private static bool IsBookingUnavailable(BookingAvailability availabilityStatus)
        {
            return availabilityStatus == BookingAvailability.PastClass ||
                   availabilityStatus == BookingAvailability.FutureClass ||
                   availabilityStatus == BookingAvailability.StartingSoon;
        }


        private static string BuildClassTime(ScheduledClass scheduledClass)
        {
            const string timeFormat = "h:mm tt"; //1:00pm
            return scheduledClass.StartTime.ToString(timeFormat);
        }
    }
}
