using System;
using Exerp.Api.WebServices.BookingAPI;
// ReSharper disable InconsistentNaming

namespace Exerp.Api.Interfaces.WebServices
{
    internal interface IBookingApi
    {
        booking[] findClassesByMultipleValues(findClassesByMultipleValues findClasses);

        createOrUpdateParticipationResult createOrUpdateParticipation(createOrUpdateParticipationParams parameters);

        void cancelPersonParticipation(compositeKey participationId, compositeKey participationPersonId, userInterfaceType userInterfaceType, Boolean sendCancelMessage);

        /// <summary>
        /// Returns the not finished participations for a person
        /// </summary>
        participation[] findPersonParticipations(compositeKey personId);

        /// <summary>
        /// Returns participations for a person, old non showups and show ups
        /// </summary>
        participation[] getPersonParticipations(compositeKey personId,string fromDate,string toDate,personalBookingType personalBookingType);

        availableSeatsResult getAvailableSeats(availableSeatsParameters parameters);

        resource[] getResources(int centreId, int activityId);
    }
}
