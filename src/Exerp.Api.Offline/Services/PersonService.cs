using System;
using System.Collections.Generic;
using System.Linq;
using Exerp.Api.DataTransfer;
using Exerp.Api.Offline.SampleData;
using Exerp.Api.Interfaces.Services;

namespace Exerp.Api.Offline.Services
{

    public class PersonService : IPersonService
    {
        private readonly ISampleDataProvider _sampleDataProvider;

        public PersonService(ISampleDataProvider sampleDataProvider)
        {
            _sampleDataProvider = sampleDataProvider;
        }

        public void GetBookedClasses(string personId)
        {
            //Class ID
        }

        public void GetWaitingListClasses(string personId)
        {
            //Class ID
        }

        public Identity ValidateLoginByEmailAddress(string emailAddress, string password)
        {
            var data = ((IEnumerable<dynamic>)_sampleDataProvider.GetDataSet().people);

            var result = data.FirstOrDefault(a => string.Compare(emailAddress, (string)a.email, StringComparison.CurrentCultureIgnoreCase) == 0 && (string)a.password == password);
            
            return result == null ? null : new Identity((int)result.centre, (int)result.id);
        }

        public Identity ValidateLoginById(Identity personIdentity, string password)
        {
            throw new NotImplementedException();
        }

        public Identity CreatePersonWithDetails(PersonDetails personDetails)
        {
            return new Identity(0,0);
        }

        public Identity CreatePersonFriend(Person personDetails)
        {
            return new Identity(0, 0);
        }

        public void CreatePersonFriendLink(Identity person1Id, Identity person2Id)
        {
            throw new NotImplementedException();
        }

        public Identity UpgradeFriendPersonWithDetails(Identity personId, PersonDetails personDetails, string token)
        {
            throw new NotImplementedException();
        }

        public bool UpdatePersonPassword(Identity personIdentity, string currentPassword, string newPassword)
        {
            throw new NotImplementedException();
        }

        public bool UpdatePersonPasswordWithToken(string newPassword, string token, Identity personId)
        {
            throw new NotImplementedException();
        }

        public bool UpdatePersonPasswordWithToken(string newPassword, string token)
        {
            throw new NotImplementedException();
        }

        public bool RequestPersonPasswordReset(string emailAddress)
        {
            throw new NotImplementedException();
        }


        public void UpdatePerson(Identity personId, PersonDetails personDetails)
        {
            throw new NotImplementedException();
        }

        public PersonDetails GetPersonDetails(Identity personKey)
        {
           return new PersonDetails(){Address = new Address()};
        }

        public void UpdatePersonDetails(PersonDetails personDetails)
        {
            
        }
       
        public void UpdatePersonAddress(Identity personId, Address address)
        {
            
        }

        public FindPersonResult FindPersonByEmailAddress(string emailAddress)
        {
            throw new NotImplementedException();
        }


        public Identity FindFriendByEmailAddress(string emailAddress)
        {
            var data = ((IEnumerable<dynamic>)_sampleDataProvider.GetDataSet().people);
            var result = data.FirstOrDefault(a => string.Compare(emailAddress, (string)a.email, StringComparison.CurrentCultureIgnoreCase) == 0);
            return result == null ? null : new Identity((int)result.centre, (int)result.id);
        }

        public IEnumerable<PersonFriend> GetRecentFriends(Identity personId)
        {
            throw new NotImplementedException();
        }


        public IEnumerable<PersonFriend> GetRecentFriends()
        {
            return new List<PersonFriend>();//{new PersonFriend(){FriendId = new Identity(701,101),FirstName="John",LastName="Test"},new PersonFriend(){FriendId = new Identity(701,102),FirstName="Jason",LastName="Test"}};
        }

        public IEnumerable<PurchasedClipCard> GetActiveClipCards(Identity personId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<PurchasedClipCard> GetInActiveClipCards(Identity personId)
        {
            throw new NotImplementedException();
        }

        public Dictionary<int, IEnumerable<Participation>> GetFutureParticipations(Identity personId)
        {
            throw new NotImplementedException();
        }

        public Dictionary<int, IEnumerable<Participation>> GetPastParticipations(Identity user)
        {
            throw new NotImplementedException();
        }
    }
}
