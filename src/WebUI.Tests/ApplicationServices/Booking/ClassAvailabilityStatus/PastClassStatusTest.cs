using System;
using FluentAssertions;
using NUnit.Framework;
using PureRide.Web.ApplicationServices.Booking.ClassAvailabilityStatus;

namespace Web.Tests.ApplicationServices.Booking.ClassAvailabilityStatus
{
    [TestFixture]
    public class PastClassStatusTests
    {

        [Test]
        public void When_IsValid_DateIsInNearFuture_ReturnFalse()
        {
            var subject = new PastClassStatus();
            var result = subject.IsValid(DateTime.Now.AddMinutes(1));
            result.Should().BeFalse();
        }

        [Test]
        public void When_IsValid_DateIsNow_ReturnFalse()
        {
            var subject = new PastClassStatus();
            var result = subject.IsValid(DateTime.Now);
            result.Should().BeFalse();
        }
         
        [Test]
        public void When_IsValid_DateIsInNearPast_ReturnTrue()
        {
            var subject = new PastClassStatus();
            var result = subject.IsValid(DateTime.Now.AddMinutes(-5));
            result.Should().BeTrue();
        }

        [Test]
        public void When_IsValid_DateIsInPast_ReturnTrue()
        {
            var subject = new PastClassStatus();
            var result = subject.IsValid(DateTime.Now.AddDays(-5));
            result.Should().BeTrue();
        }


    }
}