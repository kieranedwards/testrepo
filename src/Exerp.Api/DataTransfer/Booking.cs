using System.Collections.Generic;

namespace Exerp.Api.DataTransfer
{
    public class Booking
    {
        public Identity ClassId { get; set; }
        public PersonBooking PrimaryPerson { get; set; }
        public List<PersonBooking> Friends { get; set; }
    }

    public class PersonBooking
    {
        public Identity PersonId { get; set; }
        public string SeatId { get; set; }
    }
}
