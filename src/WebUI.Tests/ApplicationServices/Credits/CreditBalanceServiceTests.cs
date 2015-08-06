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

namespace Web.Tests.ApplicationServices.Credits
{
    [TestFixture]
    public class CreditBalanceServiceTests
    {
        private Mock<IAuthenticationService> _authService;
        private Mock<IPersonService> _personService;

        [SetUp]
        public void Setup()
        {
            _authService = new Mock<IAuthenticationService>();
           _personService = new Mock<IPersonService>();
        }

        [Test]
        public void When_Getting_Balance_For_Region_And_User_Not_Logged_In_Return_Balance_0()
        {
           var subject = new CreditBalanceService(_authService.Object,_personService.Object);

            var result = subject.GetBalance("London");
            result.Should().Be(0);
        }

        [Test]
        public void When_Getting_Balance_For_Region_Only_Add_That_Regions_Credits()
        {
            _authService.Setup(a => a.GetCurrentUser()).Returns(new Identity(123, 123));
            _personService.Setup(a => a.GetActiveClipCards(It.IsAny<Identity>())).Returns(
             new[]
             {
                 new PurchasedClipCard() { Region = "London", ClipsLeft = 5,ClipsInitial = 20},
                 new PurchasedClipCard() { Region = "Scotland", ClipsLeft = 5,ClipsInitial = 20}
             }
           );

            var subject = new CreditBalanceService(_authService.Object, _personService.Object);
            var result = subject.GetBalance("Scotland");
            result.Should().Be(5);
        }

        [Test]
        public void When_Getting_Balance_For_Region_Ignore_Region_Case()
        {
            _authService.Setup(a => a.GetCurrentUser()).Returns(new Identity(123, 123));
            _personService.Setup(a => a.GetActiveClipCards(It.IsAny<Identity>())).Returns(
                new [] {new PurchasedClipCard() {Region="London",ClipsLeft=20} }
              );
            var subject = new CreditBalanceService(_authService.Object, _personService.Object);

            var result = subject.GetBalance("LONDON");

            result.Should().Be(20);
        }

        [Test]
        public void When_Getting_Balance_For_Region_Only_Include_Clips_Left()
        {
            _authService.Setup(a => a.GetCurrentUser()).Returns(new Identity(123, 123));
            _personService.Setup(a => a.GetActiveClipCards(It.IsAny<Identity>())).Returns(
             new[]
             {
                 new PurchasedClipCard() { Region = "London", ClipsLeft = 5,ClipsInitial = 20} 
             }
           );
            
            var subject = new CreditBalanceService(_authService.Object, _personService.Object);
            var result = subject.GetBalance("London");
            result.Should().Be(5);
        }

        [Test]
        public void When_Getting_Balance_For_Region_Only_With_Multi_Sum_Clips_Left()
        {
            _authService.Setup(a => a.GetCurrentUser()).Returns(new Identity(123, 123));
            _personService.Setup(a => a.GetActiveClipCards(It.IsAny<Identity>())).Returns(
             new[]
             {
                 new PurchasedClipCard() { Region = "London", ClipsLeft = 5,ClipsInitial = 20},
                 new PurchasedClipCard() { Region = "London", ClipsLeft = 5,ClipsInitial = 20}
             }
           );

            var subject = new CreditBalanceService(_authService.Object, _personService.Object);
            var result = subject.GetBalance("London");
            result.Should().Be(10);
        }

        [Test]
        public void When_Getting_Balances_And_User_Not_Logged_In_Return_Balance_0()
        {
            _authService.Setup(a => a.GetCurrentUser()).Returns((Identity)null);
            var subject = new CreditBalanceService(_authService.Object, _personService.Object);

            var result = subject.GetBalances();
            result.Should().BeEmpty();
        }

        [Test]
        public void When_Getting_Balances_And_Single_Region_Only_Include_One_Item()
        {
            _authService.Setup(a => a.GetCurrentUser()).Returns(new Identity(123, 123));
            _personService.Setup(a => a.GetActiveClipCards(It.IsAny<Identity>())).Returns(
             new[]
             {
                 new PurchasedClipCard() { Region = "London", ClipsLeft = 5,ClipsInitial = 20},
                 new PurchasedClipCard() { Region = "London", ClipsLeft = 5,ClipsInitial = 20}
             }
           );

            var subject = new CreditBalanceService(_authService.Object, _personService.Object);
            var result = subject.GetBalances();
            result.Should().HaveCount(1);
        }

        [Test]
        public void When_Getting_Balances_And_Multi_Region_Only_Include_All_Items()
        {
            _authService.Setup(a => a.GetCurrentUser()).Returns(new Identity(123, 123));
            _personService.Setup(a => a.GetActiveClipCards(It.IsAny<Identity>())).Returns(
            new[]
             {
                 new PurchasedClipCard() { Region = "London", ClipsLeft = 5,ClipsInitial = 20},
                 new PurchasedClipCard() { Region = "London", ClipsLeft = 5,ClipsInitial = 20},
                 new PurchasedClipCard() { Region = "Scotland", ClipsLeft = 5,ClipsInitial = 20}
             }
          );

            var subject = new CreditBalanceService(_authService.Object, _personService.Object);
            var result = subject.GetBalances();
            result.Should().HaveCount(2);
        }

        [Test]
        public void When_Getting_Balances_Only_Include_Clips_Left()
        {
            _authService.Setup(a => a.GetCurrentUser()).Returns(new Identity(123, 123));
            _authService.Setup(a => a.GetCurrentUser()).Returns(new Identity(123, 123));
            _personService.Setup(a => a.GetActiveClipCards(It.IsAny<Identity>())).Returns(
            new[]
             {
                 new PurchasedClipCard() { Region = "London", ClipsLeft = 5,ClipsInitial = 20},
                 new PurchasedClipCard() { Region = "London", ClipsLeft = 5,ClipsInitial = 20},
                 new PurchasedClipCard() { Region = "Scotland", ClipsLeft = 5,ClipsInitial = 20}
             }
          );

            var subject = new CreditBalanceService(_authService.Object, _personService.Object);
            var result = subject.GetBalances();
            result["London"].Should().Be(10);
            result["Scotland"].Should().Be(5);
        }


    }
}
