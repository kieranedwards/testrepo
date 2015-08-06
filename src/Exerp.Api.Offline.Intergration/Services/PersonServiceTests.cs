using System;
using Exerp.Api.DataTransfer;
using NUnit.Framework;

namespace Exerp.Api.Offline.Integration.Services
{

    [TestFixture]
    public class PersonServiceTests
    {
      
        [Test]
        public void GetBookedClasses(string personId)
        {
            //Class ID
        }

        [Test]
        public void GetWaitingListClasses(string personId)
        {
            //Class ID
        }

        [Test]
        public void ValidateLoginByEmailAddress(string emailAddress, string password)
        {
         
        }

        [Test]
        public Identity CreatePersonWithDetails(PersonDetails personDetails)
        {
            throw new NotImplementedException();
        }

        [Test]
        public void UpdatePersonWithDetails(PersonDetails personDetails)
        {
            throw new NotImplementedException();
        }

        [Test]
        public Identity ValidateLoginById(Identity personIdentity, string password)
        {
            throw new NotImplementedException();
        }

        [Test]
        public void UpdatePersonPassword(Identity personIdentity, string currentPassword, string newPassword)
        {
            throw new NotImplementedException();
        }

        [Test]
        public bool UpdatePersonPasswordWithToken(string emailAddress, string newPassword, string token)
        {
            throw new NotImplementedException();
        }

        [Test]
        public bool RequestPersonPasswordReset(string emailAddress)
        {
            throw new NotImplementedException();
        }

 
    }
}
