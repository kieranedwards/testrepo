using System.Collections.Generic;

namespace PureRide.Web.ViewModels.Booking
{
    public class ClassScheduleModel 
    {
        public string Location { get; set; }
        public IEnumerable<ClassDayModel> Days { get; set; }
        public List<ClassStatusMessage> Messages { get; set; }
    }

    public class BookingAction
    {
        public BookingAvailability Availability { get; private set; }
        public string Url { get; private set; }

        public BookingAction(BookingAvailability availability, string url)
        {
            Availability = availability;
            Url = url;
        }
    }

}
