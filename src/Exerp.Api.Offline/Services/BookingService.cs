using System;
using System.Collections.Generic;
using System.Linq;
using Exerp.Api.DataTransfer;
using Exerp.Api.Interfaces.Services;
using Exerp.Api.Offline.SampleData;

namespace Exerp.Api.Offline.Services
{

    public class BookingService : IBookingService
    {
        private readonly ISampleDataProvider _sampleDataProvider;

        public BookingService(ISampleDataProvider sampleDataProvider)
        {
            _sampleDataProvider = sampleDataProvider;
        }

        public IEnumerable<ScheduledClass> GetClassListForCentre(int centreId, DateTime fromDate, DateTime toDate)
        {
            var data =
                (IEnumerable<dynamic>)
                    ((IEnumerable<dynamic>) _sampleDataProvider.GetDataSet().centres).First(a => a.id == centreId)
                        .classes;
            return data.Select(a =>
            {
                var startTime =
                    DateTime.Today.Add(new TimeSpan((int)a.dateOffset, (int)a.startHours, (int)a.startMinutes, 0));
                return new ScheduledClass()
                {
                    BookingId = new Identity(centreId, (int) a.id),
                    BookedCount = (int) a.bookedCount,
                    ClassCapacity = (int) a.classCapacity,
                    InstructorName = a.instructorName,
                    Name = a.name,
                    Description = a.description,
                    RoomName = a.roomName,
                    StartTime = startTime,
                    EndTime = startTime.AddMinutes(40),
                    WaitingListCount = a.waitingListCount
                };
            });
        }

        public ScheduledClass GetClassById(Identity classId)
        {
            var classes = GetClassListForCentre(classId.CentreId, DateTime.MinValue, DateTime.MaxValue).Where(a => a.BookingId.Equals(classId));
            return classes.SingleOrDefault();
        }

        public IEnumerable<Seat> GetAvaliableStudioSeats(Identity classId)
        {
            var seats = new List<Seat>();

            var rowId = 0;
            decimal columnId = 0;
            decimal offSet = 0;
            
            for (int seatId = 0; seatId < 50; seatId++)
            {
                if (seatId % 10 == 0)
                {
                    rowId += 1;
                    columnId = (rowId % 2) == 0 ? 0 : 0.5m;
                }

                if (seatId%3 == 0)
                {
                    seats.Add(new Seat() { Name = (seatId + 1).ToString(), CoordinateX = columnId + offSet, CoordinateY = rowId });
                }

                columnId += 1;
            }

            return seats;
        }

        public IEnumerable<Seat> GetStudioSeats(int activityId, int centreId, string studioName)
        {
            throw new NotImplementedException();
        }

        public void BookStudioSeats(Booking model)
        {
            throw new NotImplementedException();
        }

        public Dictionary<int, IEnumerable<Participation>> GetPersonParticipations(Identity personId)
        {
            throw new NotImplementedException();
        }


        public IEnumerable<Seat> GetStudioSeats(Identity classId)
        {
            var seats = new List<Seat>();

            var rowId = 0;
            decimal columnId = 0;
            decimal offSet = 0;
            for (int seatId = 0; seatId < 50; seatId++)
            {
                if (seatId % 10 == 0)
                {
                    rowId += 1;
                    columnId = (rowId % 2) == 0 ? 0 : 0.5m;
                }

                seats.Add(new Seat() { Name = (seatId + 1).ToString(), CoordinateX = columnId + offSet, CoordinateY = rowId });

                columnId += 1;
            }

            return seats;
        }

        public void BookStudioSeat()
        {
            throw new NotImplementedException();
        }
 
        public bool PersonIsBookedOnClass(Identity personId, Identity classId)
        {
            throw new NotImplementedException();
        }

        public void CancelClass(Identity personId, Identity participationId)
        {
            throw new NotImplementedException();
        }
    }
}
