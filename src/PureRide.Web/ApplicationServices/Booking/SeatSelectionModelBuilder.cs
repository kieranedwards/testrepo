using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Exerp.Api.DataTransfer;
using Exerp.Api.Interfaces.Services;
using PureRide.Web.ApplicationServices.Account;
using PureRide.Web.ApplicationServices.Credits;
using PureRide.Web.Helpers;
using PureRide.Web.ViewModels.Booking;

namespace PureRide.Web.ApplicationServices.Booking
{
    public class SeatSelectionModelBuilder : ISeatSelectionModelBuilder
    {
        private readonly ICentreService _centreService;
        private readonly ICreditBalanceService _creditBalanceService;
        private readonly IBookingService _bookingService;
        private readonly IScheduledClassModelAdapter _scheduledClassModelAdapter;
        private readonly IBookingAvailabilityMessageBuilder _bookingAvailabilityMessageBuilder;
        private readonly IPersonService _personService;
        private readonly IAuthenticationService _authenticationService;

        public SeatSelectionModelBuilder(ICentreService centreService, 
            ICreditBalanceService creditBalanceService,
            IBookingService bookingService, 
            IScheduledClassModelAdapter scheduledClassModelAdapter, 
            IBookingAvailabilityMessageBuilder bookingAvailabilityMessageBuilder,
            IPersonService personService, IAuthenticationService authenticationService)
        {
            _centreService = centreService;
            _creditBalanceService = creditBalanceService;
            _bookingService = bookingService;
            _scheduledClassModelAdapter = scheduledClassModelAdapter;
            _bookingAvailabilityMessageBuilder = bookingAvailabilityMessageBuilder;
            _personService = personService;
            _authenticationService = authenticationService;
        }

        //this code needs to work both when there is a user logged in and logged out
        public SeatSelectionModel BuildModel(string location, int classId)
        {
            location = location.FromStudioSlug();

            var centre = _centreService.GetActiveCentreByName(location);          
            if(centre==null)
                return null;

            var classDetails = _bookingService.GetClassById(new Identity(centre.CenterId, classId));
            if (classDetails == null)
                return null;

            var scheduledClass = _scheduledClassModelAdapter.Create(classDetails);

            var result = new SeatSelectionModel
            {
                Region = centre.Region,
                Class = scheduledClass,
                CreditBalance = _creditBalanceService.GetBalance(centre.Region),
                Studio = new StudioModel() { Location = centre.WebName, Seats = GetStudioAvaliableSeats(classDetails) },
                Message = _bookingAvailabilityMessageBuilder.BuildMessages().SingleOrDefault(a => a.AvailabilityStatus==scheduledClass.AvailabilityStatus),
                RecentFriends = GetRecentFriends()
            };

            //todo:load seats then overlay avlaibleity from exerp and from local data

            return result;
        }

        private IEnumerable<FriendModel> GetRecentFriends()
        {
            var user = _authenticationService.GetCurrentUser();

            if (user == null)
                return Enumerable.Empty<FriendModel>();
            
            return _personService.GetPersonDetails(_authenticationService.GetCurrentUser())
                        .Friends.Select(Mapper.Map<FriendModel>);
        }

        private IEnumerable<SeatModel> GetStudioAvaliableSeats(ScheduledClass scheduledClass)
        {
            var availableStudioSeats = _bookingService.GetAvaliableStudioSeats(scheduledClass.BookingId);

            var allSeats = _bookingService.GetStudioSeats(scheduledClass.ActivityId, scheduledClass.BookingId.CentreId, scheduledClass.RoomName);
            return allSeats.Select( 
                    a =>
                        new SeatModel()
                        {
                            SeatId = a.Name,
                            X = a.CoordinateX,
                            Y = a.CoordinateY,
                            IsInstructor =a.IsInstructor,
                            IsAvailable = availableStudioSeats.Any(b => string.CompareOrdinal(a.Name, b.Name) == 0)
                        });
        }
    }
 
    public interface ISeatSelectionModelBuilder
    {
        SeatSelectionModel BuildModel(string location, int classId);
    }
}
