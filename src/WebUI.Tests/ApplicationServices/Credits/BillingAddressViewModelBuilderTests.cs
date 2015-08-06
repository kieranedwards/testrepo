using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Exerp.Api.DataTransfer;
using Exerp.Api.Interfaces.Services;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using PureRide.Web.ApplicationServices.Account;
using PureRide.Web.ApplicationServices.Credits;
using PureRide.Web.ViewModels.Credits;

namespace Web.Tests.ApplicationServices.Credits
{
    [TestFixture]
    public class BillingAddressViewModelBuilderTests
    {
        private Mock<IPersonService> _personService;

        [SetUp]
        public void Setup()
        {
            _personService = new Mock<IPersonService>();
            _personService.Setup(a => a.GetPersonDetails(It.IsAny<Identity>()))
                .Returns(new PersonDetails() {Address = new Address(){Address1 = "Line1"}});        
        }


        [Test]
        public void When_Building_Model_And_Not_Logged_In_Error()
        {
            var authService = new Mock<IAuthenticationService>();
            var subject = new BillingAddressViewModelBuilder(authService.Object, _personService.Object);
            Action action = () => subject.BuildModel();
            action.ShouldThrow<NotSupportedException>();
        }

        [Test]
        public void When_Building_Model_And_Logged_In_Call_Api()
        {
            var authService = new Mock<IAuthenticationService>();
            authService.Setup(a => a.GetCurrentUser()).Returns(new Identity(123, 123));

            var subject = new BillingAddressViewModelBuilder(authService.Object, _personService.Object);
            var result = subject.BuildModel();
            
            result.Should().NotBeNull();
            result.Address1.Should().BeEquivalentTo("Line1");
        }

        [Test]
        public void When_Updating_From_Model_And_Not_Logged_In_Error()
        {
            var authService = new Mock<IAuthenticationService>();
            var subject = new BillingAddressViewModelBuilder(authService.Object, _personService.Object);
            Action action = () => subject.UpdateFromModel(new BillingAddressModel());
            action.ShouldThrow<NotSupportedException>();
        }


        [Test]
        public void When_Updating_From_Model_And_Not_Logged_In_Call_Api()
        {
            Address inputAddress = null;
            Identity inputUser = null;

            _personService.Setup(a => a.UpdatePersonAddress(It.IsAny<Identity>(), It.IsAny<Address>()))
                .Callback<Identity, Address>((id,address) => { 
                    inputUser = id;
                    inputAddress = address;
                }).Verifiable();
            
            var authService = new Mock<IAuthenticationService>();
            authService.Setup(a => a.GetCurrentUser()).Returns(new Identity(123, 123));

            var subject = new BillingAddressViewModelBuilder(authService.Object, _personService.Object);
            subject.UpdateFromModel(new BillingAddressModel(){Address1="Test"});
            
            _personService.Verify(a => a.UpdatePersonAddress(It.IsAny<Identity>(), It.IsAny<Address>()));
            inputUser.CentreId.Should().Be(123);
            inputAddress.Address1.ShouldBeEquivalentTo("Test");
        }

    }
}
