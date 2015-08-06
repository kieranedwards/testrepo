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
using PureRide.Web.ApplicationServices.Location;

namespace Web.Tests.ApplicationServices.Location
{
    [TestFixture]
    public class LocationViewModelBuilderTests
    {
        private Mock<ICentreService> _centreService;

        [SetUp]
        public void SetUp()
        {
            _centreService = new Mock<ICentreService>();
            _centreService.Setup(a => a.GetActiveRegions()).Returns(new []{"London","Scotland"});
            _centreService.Setup(a => a.GetActiveCentresWithDetails()).Returns(new Dictionary<string, Centre>(){
                {
                    "London1",new Centre() {CenterId=123,WebName="London1"}
                 },
                 {
                    "London2",new Centre() {CenterId=124,WebName="London2"}
                 },
                 {
                    "Scotland1",new Centre() {CenterId=125,WebName="Scotland1"}
                 }
            });
            _centreService.Setup(a => a.GetActiveCentreByName("London1")).Returns(new Centre() { CenterId = 123, WebName = "London1" });
        }

        [Test]
        public void When_BuildModelForAll_Return_Full_List_Locations()
        {
            var subject = new LocationViewModelBuilder(_centreService.Object);
            var result = subject.BuildModelForAll();
            result.Locations.Should().HaveCount(3);
        }

        [Test]
        public void When_BuildModelForAll_Return_Full_List_Regions()
        {
            var subject = new LocationViewModelBuilder(_centreService.Object);
            var result = subject.BuildModelForAll();
            result.Regions.Should().HaveCount(2);
        }

        [Test]
        public void When_BuildModelForLocation_Return_Single_Locations()
        {
            var subject = new LocationViewModelBuilder(_centreService.Object);
            var result = subject.BuildModelForLocation("London1");
            result.Should().NotBeNull();
        }

        [Test]
        public void When_BuildModelForLocation_For_Invalid_Location_Return_Null()
        {
            var subject = new LocationViewModelBuilder(_centreService.Object);
            var result = subject.BuildModelForLocation("Invalid");
            result.Should().BeNull();
        }

        [Test]
        public void When_BuildModelForLocation_For_Empty_Location_Return_Null()
        {
            var subject = new LocationViewModelBuilder(_centreService.Object);
            var result = subject.BuildModelForLocation("");
            result.Should().BeNull();
        }
  
    }
}
