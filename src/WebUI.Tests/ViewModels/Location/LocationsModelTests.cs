using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using PureRide.Web.ViewModels.Location;

namespace Web.Tests.ViewModels.Location
{
    [TestFixture]
    public class LocationsModelTests
    {
        
        [Test]
        public void When_Create_Model_Initialize_Collections()
        {
            var subject = new LocationsModel();
            subject.Locations.Should().NotBeNull();
            subject.Regions.Should().NotBeNull();
        }

    }
}
