using System.Linq;
using Autofac;
using Exerp.Api.Interfaces.Services;
using FluentAssertions;
using NUnit.Framework;

namespace Exerp.Api.Offline.Integration.Services
{
    [TestFixture]
    public class CentreServiceTests
    {

        private ICentreService _centreService;

        [TestFixtureSetUp]
        public void Init()
        {
            using (var scope = DependencyConfiguration.RegisterDependencies().BeginLifetimeScope())
            {
                _centreService = scope.Resolve<ICentreService>();
            }
        }

        [Test]
        public void When_GetActiveRegions_ContainsLondon()
        {
            var result = _centreService.GetActiveRegions().ToList();
            result.Should().NotBeEmpty();
            result.Should().Contain("London");
        }

        [Test]
        public void When_GetActiveCentresWithDetails_Contains_London()
        {
            var result = _centreService.GetActiveCentresWithDetails().Where(a => a.Value.Region == "London");
            result.Should().NotBeEmpty();
            result.First().Value.AddressLine1.Should().NotBeNullOrWhiteSpace();
        }

        [Test]
        public void GetClassListForCentre()
        {
           
        }

        [Test]
        public void GetStudioSeats()
        {
            //x,y of each seat
            //is avalible for each seat
        }

        [TestCase("rIver street")]
        [TestCase("river street")]
        [TestCase("River Street")]
        public void When_GetActiveCentreByName_MixedCases(string regionName)
        {
            var result = _centreService.GetActiveCentreByName(regionName);
            result.Should().NotBeNull();
            result.CenterId.Should().BeGreaterThan(0);        
        }

    }
}
