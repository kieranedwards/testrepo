using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Exerp.Api.DataTransfer;
using Exerp.Api.Interfaces.Services;
using Exerp.Api.Interfaces.WebServices;
using Exerp.Api.WebServices.BookingAPI;

namespace Exerp.Api.Services
{
    internal class BookingService : IBookingService
    {
        private readonly IBookingApi _bookingApi;

        public BookingService(IBookingApi bookingApi)
        {
            _bookingApi = bookingApi;
        }

        public IEnumerable<ScheduledClass> GetClassListForCentre(int centreId,DateTime fromDate,DateTime toDate)
        {
            var settings = new findClassesByMultipleValues()
            {
                centers = new int?[] { centreId },
                dateInterval =
                    new interval()
                    {
                        from = fromDate.ToString(Constants.ApiDateFormat),
                        to = toDate.ToString(Constants.ApiDateFormat)
                    }
            };

            var apiResult = _bookingApi.findClassesByMultipleValues(settings);
            return apiResult.Select(Mapper.Map<ScheduledClass>).ToList();
        }

        public ScheduledClass GetClassById(Identity classId)
        {
            //todo:cache call so I can get by Id later
            return GetClassListForCentre(classId.CentreId, DateTime.Today, DateTime.Today.AddDays(7)).SingleOrDefault(a => a.BookingId.EntityId == classId.EntityId);
        }

        public IEnumerable<Seat> GetAvaliableStudioSeats(Identity classId)
        {
            var seats = _bookingApi.getAvailableSeats(new availableSeatsParameters()
            {
                bookingKey = new compositeKey()
                {
                    center = classId.CentreId,
                    id = classId.EntityId
                },
                centerId = classId.CentreId,
                centerIdSpecified = true
            }).seats;

            return seats.Select(Mapper.Map<Seat>);
        }
        
        public IEnumerable<Seat> GetStudioSeats(int activityId, int centreId, string studioName)
        {
            var resources = _bookingApi.getResources(centreId, activityId);
            var studio = resources.FirstOrDefault(a => a.name == studioName);

            if (studio == null)
                return Enumerable.Empty<Seat>();

            var instructorSeat = new Seat
            {
                IsInstructor = true,
                CoordinateY = new decimal(studio.instructorRow),
                CoordinateX = new decimal(studio.instructorPosition)
            };

            return studio.seats.Select(Mapper.Map<Seat>).Concat(new []{instructorSeat});
        }

        public void BookStudioSeats(Booking model)
        {
            var participators = new List<participator>
            {
                new participator()
                {
                    personId = Mapper.Map<compositeKey>(model.PrimaryPerson.PersonId),
                    seatName = model.PrimaryPerson.SeatId
                }
            };
            participators.AddRange(model.Friends.Select(friends => new participator() {personId = Mapper.Map<compositeKey>(friends.PersonId), seatName = friends.SeatId}));

            _bookingApi.createOrUpdateParticipation(new createOrUpdateParticipationParams()
            {
                bookingId = Mapper.Map<compositeKey>(model.ClassId),
                bookingOwnerPersonId = Mapper.Map<compositeKey>(model.PrimaryPerson.PersonId),
                participators = participators.ToArray(),
                sendConfirmationMessage = true,
                userInterfaceType = userInterfaceType.WEB
            });
        }

        public bool PersonIsBookedOnClass(Identity personId, Identity booking)
        {
            return _bookingApi.findPersonParticipations(new compositeKey() { center = personId.CentreId, id = personId.EntityId }).Any(a => booking.CentreId == a.booking.bookingId.center && booking.EntityId == a.booking.bookingId.id);
        }

        public void CancelClass(Identity personId,Identity participationId)
        {
            _bookingApi.cancelPersonParticipation(Mapper.Map<compositeKey>(participationId),Mapper.Map<compositeKey>(personId), userInterfaceType.WEB, true);
        }
    }
}