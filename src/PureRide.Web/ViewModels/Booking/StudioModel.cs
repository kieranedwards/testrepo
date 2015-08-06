using System.Collections.Generic;

namespace PureRide.Web.ViewModels.Booking
{
    public class StudioModel
    {
        public string Location { get; set; }
        public IEnumerable<SeatModel> Seats { get; set; }
    }
}
