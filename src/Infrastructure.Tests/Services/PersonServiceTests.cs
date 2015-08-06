using System;
using Exerp.Api.DataTransfer;
using Exerp.Api.Interfaces.Services;
using Exerp.Api.Interfaces.WebServices;
using Exerp.Api.Services;
using Exerp.Api.WebServices.PersonAPI;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using compositeKey = Exerp.Api.WebServices.SelfServiceAPI.compositeKey;

namespace Exerp.Api.Tests.Services
{ 
    [TestFixture]
    public class PersonServiceTests
    {
        private Identity _testPerson;
        private Mock<ISelfServiceApi> _selfServiceApi;
        private Mock<IPersonApi> _personApi;
        private Mock<ICentreService> _centreService;

        [SetUp]
        public void Setup()
        {
            _testPerson = new Identity(701, 300);
            var testApiPerson = new compositeKey()
            {
                center = _testPerson.CentreId,
                id = _testPerson.EntityId
            };
            _selfServiceApi = new Mock<ISelfServiceApi>();
            _selfServiceApi.Setup(a => a.findPersonByEmail("test@test.com")).Returns(testApiPerson);
            _selfServiceApi.Setup(a => a.validatePassword(It.Is<compositeKey>(
                data => data.center == _testPerson.CentreId 
                        && data.id == _testPerson.EntityId
                ), "password")).Returns(true);

            _selfServiceApi.Setup(a => a.updatePassword(It.Is<compositeKey>(
                data => data.center == _testPerson.CentreId
                        && data.id == _testPerson.EntityId
                ), "password", It.IsAny<string>())).Returns(true);

            _personApi = new Mock<IPersonApi>();
            _personApi.Setup(a => a.getPersonDetail(It.IsAny<personKey>())).Returns(new personDetail()
            {
                extendedAttributes = new extendedAttribute[] { new extendedAttribute() { id=Constants.TermsEaKey,value="true"} }
            });
            _centreService = new Mock<ICentreService>();

        }

        private PersonService BuildPersonApi()
        {
            return new PersonService(_selfServiceApi.Object, _personApi.Object, _centreService.Object, null, null, null, null);
        }

        [Test]
        public void When_ValidateLoginByEmailAddress_EmailIsNotValid()
        {
            var subject = BuildPersonApi();
            var result = subject.ValidateLoginByEmailAddress("test","password");
            result.ShouldBeEquivalentTo(null);
        }

        [Test]
        public void When_ValidateLoginByEmailAddress_PasswordIsNotValid()
        {
            var subject = BuildPersonApi();
            var result = subject.ValidateLoginByEmailAddress("test@test.com", "abc");
            result.ShouldBeEquivalentTo(null);
        }

        [Test]
        public void When_ValidateLoginById_PasswordIsNotValid()
        {
            var subject = BuildPersonApi();
            var result = subject.ValidateLoginById(_testPerson, "abc");
            result.ShouldBeEquivalentTo(null);
        }

        [Test]
        public void When_ValidateLoginById_PasswordIsValid()
        {
            var subject = BuildPersonApi();
            var result = subject.ValidateLoginById(_testPerson, "password");
            result.ShouldBeEquivalentTo(_testPerson);
        }
 

        [Test]
        public void When_UpdatePersonPassword_PasswordIsNotValid()
        {
            var subject = BuildPersonApi();
            var result = subject.UpdatePersonPassword(_testPerson, "invalid", "abc2");
            result.ShouldBeEquivalentTo(false);
        }

        [Test]
        public void When_UpdatePersonPassword_PasswordIsValid()
        {
            var subject = BuildPersonApi();
            var result = subject.UpdatePersonPassword(_testPerson, "password", "abc2");
            result.ShouldBeEquivalentTo(true);
        }

        [Test]
        public void When_UpdatePersonPasswordWithToken_PasswordIsValid()
        {
            var subject = BuildPersonApi();
            var result = subject.UpdatePersonPasswordWithToken("password", "token", _testPerson);
            result.ShouldBeEquivalentTo(true);
        }

        [Test]
        public void When_UpdatePersonPasswordWithToken_PasswordIsNotValid()
        {
            var subject = BuildPersonApi();
            var result = subject.UpdatePersonPasswordWithToken("abc", "token", _testPerson);
            result.ShouldBeEquivalentTo(true);
        }

        [Test]
        public void When_UpdatePersonPasswordWithToken_TokenIsNotValid()
        {
            var subject = BuildPersonApi();
            var result = subject.UpdatePersonPasswordWithToken("password", "invalid", _testPerson);
            result.ShouldBeEquivalentTo(true);
        }

        [Test]
        public void When_UpdatePersonPasswordWithToken_TokenIsEmpty()
        {
            var subject = BuildPersonApi();
            var result = subject.UpdatePersonPasswordWithToken("password", string.Empty, _testPerson);
            result.ShouldBeEquivalentTo(false);
        }

        [Test]
        public void When_RequestPersonPasswordReset_EmailNotFound()
        {
            var subject = BuildPersonApi();
            var result = subject.RequestPersonPasswordReset("unknown");
            result.ShouldBeEquivalentTo(false);
        }

        [Test]
        public void When_RequestPersonPasswordReset_EmailFound()
        {
            var subject = BuildPersonApi();
            var result = subject.RequestPersonPasswordReset("test@test.com");
            result.ShouldBeEquivalentTo(true);
        }

        [Test]
        public void When_Request_Person_Password_Reset_For_Friend_Show_Not_Found()
        {
            _personApi.Setup(a => a.getPersonDetail(It.IsAny<personKey>())).Returns(new personDetail()
            {
                extendedAttributes = new extendedAttribute[] { new extendedAttribute() { id = Constants.TermsEaKey, value = "" } }
            });
            var subject = BuildPersonApi();
            var result = subject.RequestPersonPasswordReset("test@test.com");
            result.ShouldBeEquivalentTo(false);
        }

    }
}
