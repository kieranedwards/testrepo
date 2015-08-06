namespace PureRide.Web.ViewModels.Booking
{
    public class ClassStatusMessage
    {

        public ClassStatusMessage(BookingAvailability availabilityStatus, string message, string title, string url = "", string urlTitle = "")
        {
            Title = title;
            AvailabilityStatus = availabilityStatus;
            Message = message;
            Url = url;
            UrlTitle = urlTitle;
        }

        public BookingAvailability AvailabilityStatus { get; private set; }
        public string Title { get; private set; }
        public string Message { get; private set; }
        public string Url { get; private set; }
        public string UrlTitle { get; private set; }
    }
}