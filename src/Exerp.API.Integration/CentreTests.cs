using System.Linq;
using Autofac;
using Exerp.Api.Interfaces.Services;
using NUnit.Framework;

namespace Exerp.API.Integration
{
    [TestFixture]
    public class CentreTests
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
        public void GetActiveRegions()
        {
            var result = _centreService.GetActiveRegions().ToList();

            Assert.IsNotEmpty(result);
            CollectionAssert.AllItemsAreUnique(result);           
        }

        [Test]
        public void GetActiveCentresWithDetails()
        {
            var result = _centreService.GetActiveCentresWithDetails().ToList();

            Assert.IsNotEmpty(result);
            CollectionAssert.AllItemsAreUnique(result);        
        }
  
    }
}
