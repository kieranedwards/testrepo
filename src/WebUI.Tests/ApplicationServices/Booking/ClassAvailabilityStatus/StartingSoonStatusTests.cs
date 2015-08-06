using System;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using PureRide.Web.ApplicationServices.Booking.ClassAvailabilityStatus;
using PureRide.Web.Configuration;

namespace Web.Tests.ApplicationServices.Booking.ClassAvailabilityStatus
{

    [TestFixture]
    public class StartingSoonStatusTests
    {

        private IScheduleSettings _scheduleSettings;
         
        [SetUp]
        public void Setup()
        {
            var settings = new Mock<IScheduleSettings>();
            settings.Setup(a => a.MinBookableMinutesBeforeStart).Returns(10);
            _scheduleSettings = settings.Object;
        }

        [Test]
        public void When_IsValid_DateIsInNearFuture_ReturnTrue()
        {
            var subject = new StartingSoonStatus(_scheduleSettings);
            var result = subject.IsValid(DateTime.Now.AddMinutes(1));
            result.Should().BeTrue();
        }

        [Test]
        public void When_IsValid_DateIsInBeyondLimit_ReturnFalse()
        {
            var subject = new StartingSoonStatus(_scheduleSettings);
            var result = subject.IsValid(DateTime.Now.AddMinutes(11));
            result.Should().BeFalse();
        }

        [Test]
        public void When_IsValid_DateIsInPast_ReturnFalse()
        {
            var subject = new StartingSoonStatus(_scheduleSettings);
            var result = subject.IsValid(DateTime.Now.AddMinutes(-10));
            result.Should().BeFalse();
        }


        [Test]
        public void When_IsValid_DateIsSameAsLimit_ReturnTrue()
        {
            var subject = new StartingSoonStatus(_scheduleSettings);
            var result = subject.IsValid(DateTime.Now.AddMinutes(10));
            result.Should().BeTrue();
        }


    }
}
