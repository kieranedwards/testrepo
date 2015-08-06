using System;
using Moq;
using NUnit.Framework;
using PureRide.Web.Configuration;
using PureRide.Web.ViewModels.Booking;

namespace Web.Tests.ApplicationServices.Booking.ClassAvailabilityStatus
{
    [TestFixture]
    public class WaitingListStatusTests
    {
        private IScheduleSettings _scheduleSettings;

        [SetUp]
        public void Setup()
        {
            var settings = new Mock<IScheduleSettings>();
            settings.Setup(a => a.MinBookableMinutesBeforeStart).Returns(10);
            _scheduleSettings = settings.Object;
        }
 
    }
}