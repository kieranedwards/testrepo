namespace PureRide.Web.ViewModels.Booking
{
    public class SeatModel
    {
        public decimal X  { get; set; }
        public decimal Y { get; set; }
        public string SeatId { get; set; }
        public bool IsAvailable { get; set; }
        public bool IsInstructor { get; set; }
    }
}