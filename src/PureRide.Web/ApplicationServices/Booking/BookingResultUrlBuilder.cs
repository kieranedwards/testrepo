using Exerp.Api.Interfaces.Services;

namespace PureRide.Web.ApplicationServices.Booking
{

    public class BookingResultUrlBuilder : IBookingResultUrlBuilder
    {
        public string Build()
        {
            /*
           //if not loged in return to login page
              //if enough credits redirect to booking complete
              //if not enough credits redirect to buy credits page

           */
            return string.Empty;
        }
    }

    public interface IBookingResultUrlBuilder
    {

        string Build();
    }
}
