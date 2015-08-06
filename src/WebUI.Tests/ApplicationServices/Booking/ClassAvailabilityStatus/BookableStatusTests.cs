using System;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using PureRide.Web.ApplicationServices.Booking.ClassAvailabilityStatus;
using PureRide.Web.Configuration;

namespace Web.Tests.ApplicationServices.Booking.ClassAvailabilityStatus
{
    [TestFixture]
    public class BookableStatusTests
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
        public void When_IsValidAlways_ReturnTrue()
        {
            var subject = new BookableStatus();
            var result = subject.IsValid(DateTime.Now.AddMinutes(1));
            result.Should().BeTrue();
        }

    }
}