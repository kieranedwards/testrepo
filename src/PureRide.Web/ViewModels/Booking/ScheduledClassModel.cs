namespace PureRide.Web.ViewModels.Booking
{
    public class ScheduledClassModel
    {
        public string ClassId { get; set; }
        public string InstructorName{ get; set; }
        public string ClassName { get; set; }
        public string ClassTime { get; set; }
        public string ClassDate { get; set; }
        public string RoomName { get; set; }

        public bool IsNearWaitingList { get; set; }
        public bool IsWaitingList  { get; set; }
        public int ClassCapacity { get; set; }
        public int BookedCount { get; set; }
        public bool IsBookingUnavailable { get; set; }

        public BookingAvailability AvailabilityStatus { get; set; }
    }

    //ordered by the order we show messages that apply
}