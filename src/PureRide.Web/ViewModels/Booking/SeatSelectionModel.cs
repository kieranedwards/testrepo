using System.Collections.Generic;

namespace PureRide.Web.ViewModels.Booking
{
    public class SeatSelectionModel 
    {
        public int CreditBalance { get; set; }
        public string Region { get; set; }
        public ScheduledClassModel Class { get; set; }
        public ClassStatusMessage Message { get; set; }
        public StudioModel Studio { get; set; }
        public bool UserIsBooked { get; set; }
        public IEnumerable<FriendModel> RecentFriends { get; set; }
    }
}
