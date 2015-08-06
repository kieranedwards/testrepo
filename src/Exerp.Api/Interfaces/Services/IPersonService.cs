using System.Collections.Generic;
using Exerp.Api.DataTransfer;

namespace Exerp.Api.Interfaces.Services
{
    public interface IPersonService
    {
        Identity ValidateLoginByEmailAddress(string emailAddress, string password);
        Identity ValidateLoginById(Identity personIdentity, string password);
        Identity CreatePersonWithDetails(PersonDetails personDetails);
        Identity CreatePersonFriend(Person personDetails);
        void CreatePersonFriendLink(Identity person1Id, Identity person2Id);

        Identity UpgradeFriendPersonWithDetails(Identity personId, PersonDetails personDetails, string token);
        bool UpdatePersonPassword(Identity personIdentity, string currentPassword, string newPassword);
        bool UpdatePersonPasswordWithToken(string newPassword, string token, Identity personId);
        bool RequestPersonPasswordReset(string emailAddress);
        void UpdatePersonAddress(Identity personId, Address address);
        void UpdatePerson(Identity personId, PersonDetails personDetails);

        PersonDetails GetPersonDetails(Identity personKey);
        FindPersonResult FindPersonByEmailAddress(string emailAddress);
        IEnumerable<PurchasedClipCard> GetActiveClipCards(Identity personId);
        IEnumerable<PurchasedClipCard> GetInActiveClipCards(Identity personId);

        /// <summary>
        /// Gets a list of participations grouped by booking id (as you may have also booked friends)
        /// </summary>
        /// <param name="personId">Person to load bookings for</param>
        /// <returns></returns>
        Dictionary<int, IEnumerable<Participation>> GetFutureParticipations(Identity personId);

        Dictionary<int, IEnumerable<Participation>> GetPastParticipations(Identity user);
    }
}