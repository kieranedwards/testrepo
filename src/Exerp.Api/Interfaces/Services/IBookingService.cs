using System;
using System.Collections.Generic;
using Exerp.Api.DataTransfer;

namespace Exerp.Api.Interfaces.Services
{
    public interface IBookingService
    {
        IEnumerable<ScheduledClass> GetClassListForCentre(int centreId, DateTime fromDate, DateTime toDate);
        ScheduledClass GetClassById(Identity classId);
        IEnumerable<Seat> GetAvaliableStudioSeats(Identity classId);
        IEnumerable<Seat> GetStudioSeats(int activityId, int centreId, string studioName);
        
        void BookStudioSeats(Booking model);
        
        bool PersonIsBookedOnClass(Identity personId, Identity classId);
        void CancelClass(Identity personId, Identity participationId);
    }

    public enum BookingResult
    {
        LoginRequired,
        NotEnoughCredits,
        Booked
    }
}