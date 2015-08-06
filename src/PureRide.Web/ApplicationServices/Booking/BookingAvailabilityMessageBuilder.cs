using System;
using System.Collections.Generic;
using PureRide.Web.Providers;
using PureRide.Web.ViewModels.Booking;

namespace PureRide.Web.ApplicationServices.Booking
{
    public class BookingAvailabilityMessageBuilder : IBookingAvailabilityMessageBuilder
    {
        private readonly IUmbracoHelperProvider _umbracoHelperProvider;

        public BookingAvailabilityMessageBuilder(IUmbracoHelperProvider umbracoHelperProvider)
        {
            _umbracoHelperProvider = umbracoHelperProvider;
        }

        public List<ClassStatusMessage> BuildMessages()
        {
            var umbracoHelper = _umbracoHelperProvider.GetHelper();
            var result = new List<ClassStatusMessage>();

            foreach (BookingAvailability bookingAvailability in Enum.GetValues(typeof(BookingAvailability)))
            {
                result.Add(GetStatusMessage(bookingAvailability, umbracoHelper));
            }
                
            return result;
        }


        private ClassStatusMessage GetStatusMessage(BookingAvailability bookingAvailability,IUmbracoHelper umbracoHelper)
        {
            return new ClassStatusMessage(
               bookingAvailability,
                umbracoHelper.GetFieldRecursive(bookingAvailability.ToString()).ToString(),
                umbracoHelper.GetFieldRecursive(string.Concat(bookingAvailability.ToString(), "Title")).ToString());
        }
    }

    public interface IBookingAvailabilityMessageBuilder
    {
        List<ClassStatusMessage> BuildMessages();
    }
}
