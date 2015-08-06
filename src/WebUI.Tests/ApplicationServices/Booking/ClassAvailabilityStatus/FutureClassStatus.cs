using System;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using PureRide.Web.ApplicationServices.Booking.ClassAvailabilityStatus;
using PureRide.Web.Configuration;
using PureRide.Web.ViewModels.Booking;

namespace Web.Tests.ApplicationServices.Booking.ClassAvailabilityStatus
{
    public class FutureClassStatusTests 
    {
        
        private IScheduleSettings _scheduleSettings;
         
        [SetUp]
        public void Setup()
        {
            var settings = new Mock<IScheduleSettings>();
            settings.Setup(a => a.MaxBookableDays).Returns(10);
            _scheduleSettings = settings.Object;
        } 
        [Test]
        public void When_IsValid_DateIsInNearFuture_ReturnFalse()
        {
            var subject = new FutureClassStatus(_scheduleSettings);
            var result = subject.IsValid(DateTime.Now.AddMinutes(1));
            result.Should().BeFalse();
        }

        [Test]
        public void When_IsValid_DateIsExactlySameAsMaxBookable_ReturnFalse()
        {
            var subject = new FutureClassStatus(_scheduleSettings);
            var result = subject.IsValid(DateTime.Now.AddDays(_scheduleSettings.MaxBookableDays));
            result.Should().BeFalse();
        }

        [Test]
        public void When_IsValid_DateIsExactlySameAsMaxBookable_ReturnTrue()
        {
            var subject = new FutureClassStatus(_scheduleSettings);
            var result = subject.IsValid(DateTime.Now.AddDays(_scheduleSettings.MaxBookableDays+1));
            result.Should().BeTrue();
        }

        [Test]
        public void When_IsValid_DateIsInDistantDistantFuture_ReturnTrue()
        {
            var subject = new FutureClassStatus(_scheduleSettings);
            var result = subject.IsValid(DateTime.Now.AddDays(_scheduleSettings.MaxBookableDays+100));
            result.Should().BeTrue();
        }

        [Test]
        public void When_IsValid_DateIsNow_ReturnFalse()
        {
            var subject = new FutureClassStatus(_scheduleSettings);
            var result = subject.IsValid(DateTime.Now);
            result.Should().BeFalse();
        }

        [Test]
        public void When_IsValid_DateIsInNearPast_ReturnFalse()
        {
            var subject = new FutureClassStatus(_scheduleSettings);
            var result = subject.IsValid(DateTime.Now.AddMinutes(-5));
            result.Should().BeFalse();
        }

        [Test]
        public void When_IsValid_DateIsInPast_ReturnFalse()
        {
            var subject = new FutureClassStatus(_scheduleSettings);
            var result = subject.IsValid(DateTime.Now.AddDays(-5));
            result.Should().BeFalse();
        }
    }
}