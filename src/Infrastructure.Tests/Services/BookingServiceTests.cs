using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting;
using Exerp.Api.DataTransfer;
using Exerp.Api.Helpers;
using Exerp.Api.Interfaces.WebServices;
using Exerp.Api.Services;
using Exerp.Api.WebServices.BookingAPI;
using FluentAssertions;
using FluentAssertions.Common;
using Moq;
using NUnit.Framework;

namespace Exerp.Api.Tests.Services
{
    [TestFixture]
    public class BookingServiceTests
    {
        private Mock<IBookingApi> _api;
        private const int ValidCentreId = 701;
        private readonly Identity _validClassId = new Identity(ValidCentreId, 1);
        private readonly Identity _validPersonId = new Identity(ValidCentreId, 2);
        private readonly Identity _validParticipationId = new Identity(ValidCentreId, 3);
        
        [SetUp]
        public void Setup()
        {
            _api = new Mock<IBookingApi>();


            var bookings = new List<booking>();
            bookings.Add(new booking()
            {
                bookingId = new compositeKey(){center= _validClassId.CentreId, id=_validClassId.EntityId},
                date = DateTime.Today.ToApiDate(), 
                startTime = DateTime.Today.AddHours(10).ToApiTime(),
                endTime = DateTime.Today.AddHours(11).ToApiTime(),
                bookedCount = 1,
                classCapacity = 30,
                description ="Test", 
                instructorName = "John",
                name ="Test Class",
                roomName ="Studio 1", 
                waitingListCount = 2
            });
            bookings.Add(new booking() { bookingId = new compositeKey() { center = _validClassId.CentreId, id = 2 }, date = DateTime.Today.AddDays(1).ToApiDate(), startTime = DateTime.Today.AddHours(10).ToApiTime() });
            bookings.Add(new booking() { bookingId = new compositeKey() { center = _validClassId.CentreId, id = 3 }, date = DateTime.Today.AddDays(2).ToApiDate(), startTime = DateTime.Today.AddHours(10).ToApiTime() });
            
            _api.Setup(a => a.findClassesByMultipleValues(It.IsAny<findClassesByMultipleValues>()))
                .Returns(bookings.ToArray);

            _api.Setup(m => m.getResources(ValidCentreId, 1))
                .Returns(new[] { new resource() { 
                    name = "Studio 1", 
                    instructorRow = 6,
                    instructorPosition = 8,
                    seats= new[]
                    {
                        new seat(){position = 1, row = 1,name="1"},
                        new seat(){position = 2, row = 1,name="2"},
                        new seat(){position = 3, row = 1,name="3"},
                        new seat(){position = 4, row = 1,name="4"},
                        new seat(){position = 5, row = 1,name="5"},
                    } },
                    new resource() { 
                    name = "Studio 2", 
                    seats= new[]
                    {
                        new seat(){position = 1, row = 1},
                        new seat(){position = 2, row = 1},
                        new seat(){position = 3, row = 1},
                    } }}).Verifiable();
        }

        [Test]
        public void When_GettingClassList_ReturnClassesInDateRange()
        {
            var subject = new BookingService(_api.Object);
            var result = subject.GetClassListForCentre(ValidCentreId, DateTime.Now, DateTime.Today.AddDays(5));

            result.Should().HaveCount(3);
        }

        [Test]
        public void When_GettingClassList_ReturnClassesDataIsMapped()
        {
            var subject = new BookingService(_api.Object);
            var result = subject.GetClassListForCentre(ValidCentreId, DateTime.Now, DateTime.Today.AddDays(5)).First();

            result.BookedCount.IsSameOrEqualTo(1);
            result.ClassCapacity.IsSameOrEqualTo(30);
            result.Description.ShouldBeEquivalentTo("Test");
            result.InstructorName.ShouldBeEquivalentTo("John");
            result.Name.ShouldBeEquivalentTo("Test Class");
            result.RoomName.ShouldBeEquivalentTo("Studio 1");
            result.StartTime.ShouldBeEquivalentTo(DateTime.Today.AddHours(10));
            result.EndTime.ShouldBeEquivalentTo(DateTime.Today.AddHours(11));
            result.WaitingListCount.IsSameOrEqualTo(2);
        }

        [Test]
        public void When_GettingClassList_ReturnEmptyListWhenNoClassesInDateRange()
        {
            _api.Setup(a => a.findClassesByMultipleValues(It.IsAny<findClassesByMultipleValues>())).Returns(new booking[0]);

            var subject = new BookingService(_api.Object);
            var result = subject.GetClassListForCentre(ValidCentreId, DateTime.Now, DateTime.Today.AddDays(5));

            result.Should().HaveCount(0);
        }

        [Test]
        public void When_GettingClassById_AndIdIsNotValid_ReturnNull()
        {
            var subject = new BookingService(_api.Object);
            var result = subject.GetClassById(new Identity(0,0));

            result.Should().BeNull();
        }

        [Test]
        public void When_GettingClassById_AndIdIsValid_ReturnClass()
        {
            var subject = new BookingService(_api.Object);
            var result = subject.GetClassById(_validClassId);

            result.Name.ShouldBeEquivalentTo("Test Class");
        }

        [Test]
        public void When_GettingStudioSeats_ApiIsCalled()
        {
            var subject = new BookingService(_api.Object);
            subject.GetStudioSeats(1,ValidCentreId,"Studio 1");

            _api.Verify(m => m.getResources(ValidCentreId, 1));
        }

        [Test]
        public void When_GettingStudioSeats_WithInvalidStudio_EmptyListIsReturned()
        {
            var subject = new BookingService(_api.Object);
            var result = subject.GetStudioSeats(1, ValidCentreId, "Studio X");

            result.Should().BeEmpty();
        }

        [Test]
        public void When_GettingStudioSeats_Coordinates_Are_Provided_For_Given_Studio()
        {                
            var subject = new BookingService(_api.Object);
            var result = subject.GetStudioSeats(1, ValidCentreId, "Studio 1")
                .Where(a=>a.IsInstructor==false).ToArray();

            result.Should().HaveCount(5);
            result.Last().CoordinateX.ShouldBeEquivalentTo(5);
            result.Last().CoordinateY.ShouldBeEquivalentTo(1);
            result.Last().Name.ShouldBeEquivalentTo("5");
        }

        [Test]
        public void When_GettingStudioSeats_Coordinates_Are_Provided_For_Instructor()
        {
            var subject = new BookingService(_api.Object);
            var result = subject.GetStudioSeats(1, ValidCentreId, "Studio 1").ToArray();

            var instructors = result.Where(a => a.IsInstructor).ToArray();
            instructors.Should().HaveCount(1);
            instructors.First().CoordinateX.ShouldBeEquivalentTo(8);
            instructors.First().CoordinateY.ShouldBeEquivalentTo(6);
            result.Last().Name.ShouldBeEquivalentTo(null);
        }


        [Test]
        public void When_GettingAvaliableStudioSeats_ApiIsCalled()
        {
            availableSeatsParameters paramsPassed = null;
            _api.Setup(a => a.getAvailableSeats(It.IsAny<availableSeatsParameters>()))
                .Returns(
                new availableSeatsResult()
                {
                    seats = new[] {
                        new seat(){position = 3, row = 1,name="3"},
                        new seat(){position = 4, row = 1,name="4"},
                        new seat(){position = 5, row = 1,name="5"}
                    }
                }
                )
                .Callback<availableSeatsParameters>(p => paramsPassed = p)
                .Verifiable();

            var subject = new BookingService(_api.Object);
            subject.GetAvaliableStudioSeats(_validClassId);

            _api.Verify(m => m.getAvailableSeats(It.IsAny<availableSeatsParameters>()));
            paramsPassed.bookingKey.id.ShouldBeEquivalentTo(_validClassId.EntityId); 
        }

        [Test]
        public void When_GettingAvaliableStudioSeats_CoordinatesAreProvided()
        {
            _api.Setup(a => a.getAvailableSeats(It.IsAny<availableSeatsParameters>()))
                .Returns(
                new availableSeatsResult()
                {
                    seats = new seat[] {
                        new seat(){position = 3, row = 1,name="3"},
                        new seat(){position = 4, row = 1,name="4"},
                        new seat(){position = 5, row = 1,name="5"}
                    }
                }
                );

            var subject = new BookingService(_api.Object);
            var result = subject.GetAvaliableStudioSeats(_validClassId).ToArray();

            result.Should().HaveCount(3);
            result.Last().CoordinateX.ShouldBeEquivalentTo(5);
            result.Last().CoordinateY.ShouldBeEquivalentTo(1);
            result.Last().Name.ShouldBeEquivalentTo("5");
        }

        [Test]
        public void When_BookStudioSeatsForSinglePerson_ApiIsCalled()
        {
            createOrUpdateParticipationParams paramsPassed = null;
            _api.Setup(a => a.createOrUpdateParticipation(It.IsAny<createOrUpdateParticipationParams>()))
                .Callback<createOrUpdateParticipationParams>(p => paramsPassed = p).Returns(new createOrUpdateParticipationResult())
                .Verifiable();

            var subject = new BookingService(_api.Object);
            subject.BookStudioSeats(new Booking(){ClassId = _validClassId,Friends=new List<PersonBooking>(),PrimaryPerson=new PersonBooking(){PersonId=_validPersonId,SeatId = "1"}});

            _api.Verify(m => m.createOrUpdateParticipation(It.IsAny<createOrUpdateParticipationParams>()));
            paramsPassed.bookingId.id.ShouldBeEquivalentTo(_validClassId.EntityId);
            paramsPassed.participators.Should().HaveCount(1);
            paramsPassed.participators.First().personId.id.ShouldBeEquivalentTo(_validPersonId.EntityId);
            paramsPassed.participators.First().seatName.ShouldBeEquivalentTo("1"); 
        }

        [Test]
        public void When_BookStudioSeatsWithFriends_ApiIsCalled()
        {
            createOrUpdateParticipationParams paramsPassed = null;
            _api.Setup(a => a.createOrUpdateParticipation(It.IsAny<createOrUpdateParticipationParams>()))
                .Callback<createOrUpdateParticipationParams>(p => paramsPassed = p).Returns(new createOrUpdateParticipationResult())
                .Verifiable();

            var subject = new BookingService(_api.Object);
            subject.BookStudioSeats(new Booking() { ClassId = _validClassId, Friends = new List<PersonBooking>(){new PersonBooking(){PersonId=new Identity(ValidCentreId,2),SeatId="2"}}, PrimaryPerson = new PersonBooking() { PersonId = _validPersonId, SeatId = "1" } });

            _api.Verify(m => m.createOrUpdateParticipation(It.IsAny<createOrUpdateParticipationParams>()));
            paramsPassed.bookingId.id.ShouldBeEquivalentTo(_validClassId.EntityId);
            paramsPassed.participators.Should().HaveCount(2);
            paramsPassed.participators.First().personId.id.ShouldBeEquivalentTo(_validPersonId.EntityId);
            paramsPassed.participators.First().seatName.ShouldBeEquivalentTo("1");
            paramsPassed.participators.Last().seatName.ShouldBeEquivalentTo("2"); 
        }


        [Test]
        public void When_CheckingPersonIsBookedOnClass_ApiIsCalled()
        {
            compositeKey personId = null;
            _api.Setup(m => m.findPersonParticipations(It.IsAny<compositeKey>())).Callback<compositeKey>(personKey => personId = personKey)
                .Returns(new []{new participation(){booking = new booking(){bookingId = new compositeKey()}}}).Verifiable();

            var subject = new BookingService(_api.Object);
            subject.PersonIsBookedOnClass(_validPersonId, _validParticipationId);

            _api.Verify(m => m.findPersonParticipations(It.IsAny<compositeKey>()));
            personId.id.ShouldBeEquivalentTo(_validPersonId.EntityId);
        }

        [Test]
        public void When_CheckingPersonIsBookedOnClass_IfTheyAreBooked_Return_True()
        {
            var validBookingId = 99;
            _api.Setup(m => m.findPersonParticipations(It.IsAny<compositeKey>()))
                .Returns(new[] { new participation() { booking = new booking() { bookingId = new compositeKey() { center = ValidCentreId, id = validBookingId } } } });

            var subject = new BookingService(_api.Object);
            var result = subject.PersonIsBookedOnClass(_validPersonId, new Identity(ValidCentreId, validBookingId));

            result.Should().BeTrue();
        }

        [Test]
        public void When_CheckingPersonIsBookedOnClass_IfTheyAreNotBooked_Return_False()
        {
            var validBookingId = 99;
            _api.Setup(m => m.findPersonParticipations(It.IsAny<compositeKey>()))
                .Returns(new[] { new participation() { booking = new booking() { bookingId = new compositeKey() { center = ValidCentreId, id = validBookingId } } } });

            var subject = new BookingService(_api.Object);
            var result = subject.PersonIsBookedOnClass(_validPersonId, new Identity(ValidCentreId, 0));

            result.Should().BeFalse();
        }

        [Test]
        public void When_Canceling_ApiIsCalled()
        {
            compositeKey personId = null;
            compositeKey participationId = null;
            _api.Setup(m => m.cancelPersonParticipation(It.IsAny<compositeKey>(), It.IsAny<compositeKey>(),userInterfaceType.WEB, true))
                .Callback<compositeKey, compositeKey, userInterfaceType, bool>((participationKey, personKey, type, canCancel) =>
                {
                    personId = personKey;
                    participationId = participationKey;
                })
                .Verifiable();

            var subject = new BookingService(_api.Object);
            subject.CancelClass(_validPersonId, _validParticipationId);

            _api.Verify(m => m.cancelPersonParticipation(It.IsAny<compositeKey>(), It.IsAny<compositeKey>(),userInterfaceType.WEB, true));
            personId.id.ShouldBeEquivalentTo(_validPersonId.EntityId);
            participationId.id.ShouldBeEquivalentTo(_validParticipationId.EntityId);
        }

    }
}
