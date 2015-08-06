using System;
using System.Collections.Generic;
using System.Linq;
using Exerp.Api.DataTransfer;
using Exerp.Api.Interfaces.Services;
using PureRide.Web.ApplicationServices.Account;
using PureRide.Web.ViewModels.Booking;

namespace PureRide.Web.ApplicationServices.Booking
{
    public class BookingManagerService : IBookingManagerService
    {
        private readonly IBookingService _bookingService;
        private readonly IPersonService _personService;
        private readonly IAuthenticationService _authenticationService;

        public BookingManagerService(IBookingService bookingService, 
                                     IPersonService personService,
                                     IAuthenticationService authenticationService)
        {
            _bookingService = bookingService;
            _personService = personService;
            _authenticationService = authenticationService;
        }

        public void BookClass(BookingDetailsModel model)
        {
            var user = _authenticationService.GetCurrentUser();

            var booking = new Exerp.Api.DataTransfer.Booking
            {
                ClassId = new Identity(model.ClassId),
                PrimaryPerson = new PersonBooking() {PersonId = user, SeatId = model.SeatId},
                Friends =  model.Friends == null ? new List<PersonBooking>() :
                    model.Friends.Select(a => new PersonBooking() {PersonId = new Identity(a.FriendId), SeatId = a.SeatId}).ToList()
            };

            _bookingService.BookStudioSeats(booking);
        }

        public bool PersonIsBookedAnyClass(Identity identity, DateTime startTime, DateTime endTime)
        {
            var classes = _personService.GetFutureParticipations(identity);
            return classes.Any(a=> startTime<=a.Value.First().ScheduledClass.EndTime && a.Value.First().ScheduledClass.StartTime<=endTime);//todo test overlaps  senerios
        }

        public void CancelClass(BookingCancelModel model)
        {
            _bookingService.CancelClass(new Identity(model.PersonId), new Identity(model.ParticipationId));
        }
    }
    
    public interface IBookingManagerService
    {
        void BookClass(BookingDetailsModel model);
        bool PersonIsBookedAnyClass(Identity identity, DateTime minValue, DateTime maxValue);
        void CancelClass(BookingCancelModel model);
    }
}
