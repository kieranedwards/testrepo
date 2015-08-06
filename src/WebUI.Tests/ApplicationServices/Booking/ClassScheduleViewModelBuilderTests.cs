using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Exerp.Api.DataTransfer;
using Exerp.Api.Interfaces.Services;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using PureRide.Web.ApplicationServices.Booking;
using PureRide.Web.ApplicationServices.Booking.ClassAvailabilityStatus;
using PureRide.Web.Configuration;
using PureRide.Web.ViewModels.Booking;

namespace Web.Tests.ApplicationServices.Booking
{
    [TestFixture]
    public class ClassScheduleViewModelBuilderTests
    {

        private Mock<ICentreService> _centreService;
        private Mock<IScheduleSettings> _scheduleSettings;
        private Mock<IScheduledClassModelAdapter> _classAdapter;

        private const string ValidLocation = "London Street";
        private const int CentreId = 1;

        [SetUp]
        public void Setup()
        {
            _centreService = new Mock<ICentreService>();
            _scheduleSettings = new Mock<IScheduleSettings>();
            _scheduleSettings.Setup(a => a.MaxBookableDays).Returns(30);
            _classAdapter = new Mock<IScheduledClassModelAdapter>();

            var allwaysTrueStatus = new Mock<IClassAvailabilityStatus>();
            allwaysTrueStatus.Setup(a => a.IsValid(It.IsAny<DateTime>())).Returns(true);
            _classAdapter.Setup(a=>a.Create(It.IsAny<ScheduledClass>())).Returns((ScheduledClass a)=>new ScheduledClassModel(){ClassId=a.BookingId.ToString()});

            _centreService.Setup(a => a.GetActiveCentreByName(ValidLocation)).Returns(new Centre() { CenterId = CentreId, WebName = ValidLocation });
            _centreService.Setup(a => a.GetActiveCentresWithDetails()).Returns(new Dictionary<string, Centre>() { { ValidLocation, new Centre() { CenterId = CentreId, WebName = ValidLocation } } });
            
         }

        private IBookingService MakeBookingServiceForClasses(IEnumerable<ScheduledClass> classList)
        {
            var bookingService = new Mock<IBookingService>();
            bookingService.Setup(a => a.GetClassListForCentre(CentreId, It.IsAny<DateTime>(), It.IsAny<DateTime>())).Returns(classList);
            return bookingService.Object;
        }

        [Test]
        public void When_BuildModel_InValidLocation_NoDataIsReturned()
        {
            var classes = new List<ScheduledClass>() {new ScheduledClass()};
            var subject = new ClassScheduleViewModelBuilder(_centreService.Object, MakeBookingServiceForClasses(classes), _scheduleSettings.Object,_classAdapter.Object);
            var result = subject.BuildModel("InValid");
            result.Should().BeNull();
        }

        [Test]
        public void When_BuildModel_RangeOfStartDates_AreGrouped()
        {
            var classes = new List<ScheduledClass>() { new ScheduledClass() { StartTime = DateTime.Now.AddDays(1) }, new ScheduledClass() { StartTime = DateTime.Now.AddDays(1) }, new ScheduledClass() { StartTime = DateTime.Now.AddDays(2) } };
            var subject = new ClassScheduleViewModelBuilder(_centreService.Object, MakeBookingServiceForClasses(classes),_scheduleSettings.Object,_classAdapter.Object);
            var result = subject.BuildModel(ValidLocation);
            result.Days.Should().HaveCount(2);
        }

        [Test]
        public void When_BuildModel_RangeOfStartDates_DaysAreOrdered()
        {
            var classes = new List<ScheduledClass>() { new ScheduledClass() { StartTime = DateTime.Now.AddDays(1) }, new ScheduledClass() { StartTime = DateTime.Now.AddDays(5) }, new ScheduledClass() { StartTime = DateTime.Now.AddDays(2) } };
            var subject = new ClassScheduleViewModelBuilder(_centreService.Object, MakeBookingServiceForClasses(classes),_scheduleSettings.Object,_classAdapter.Object);
            var result = subject.BuildModel(ValidLocation);
            result.Days.Should().BeInAscendingOrder(a=>a.Date);
        }

        [Test]
        public void When_BuildModel_RangeOfStartDates_ClassesAreOrderedByTime()
        {
            var classes = new List<ScheduledClass>() { 
                new ScheduledClass()
            {
                BookingId=new Identity(CentreId,3), StartTime = DateTime.Now.AddHours(1)
            }, new ScheduledClass()
            {
                BookingId=new Identity(CentreId,2), StartTime = DateTime.Now.AddHours(5)
            }, new ScheduledClass()
            {
                BookingId=new Identity(CentreId,1), StartTime = DateTime.Now.AddHours(2)
            } 
            };

            var subject = new ClassScheduleViewModelBuilder(_centreService.Object, MakeBookingServiceForClasses(classes),_scheduleSettings.Object,_classAdapter.Object);
            var result = subject.BuildModel(ValidLocation);
            result.Days.Single()
                .Classes.Select(a => a.ClassId)
                .ShouldAllBeEquivalentTo(new List<string>()
                {
                    "1:3",
                    "1:1",
                    "1:2",
                });
        }
    }
}
