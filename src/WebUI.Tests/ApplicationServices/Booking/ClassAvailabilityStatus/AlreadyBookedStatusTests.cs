using Moq;
using NUnit.Framework;
using PureRide.Web.Configuration;

namespace Web.Tests.ApplicationServices.Booking.ClassAvailabilityStatus
{
    [TestFixture]
    public class AlreadyBookedStatusTest
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