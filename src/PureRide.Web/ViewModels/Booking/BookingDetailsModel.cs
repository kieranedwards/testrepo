using System;
using System.Collections.Generic;

namespace PureRide.Web.ViewModels.Booking
{
    public class BookingDetailsModel
    {
        public string ClassId { get; set; }
        public string SeatId { get; set; }

        public List<BookingDetailFriendModel> Friends { get; set; }
    }
}
