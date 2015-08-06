using System;
using System.Collections.Generic;
using System.Linq;
using Exerp.Api.DataTransfer;
using Exerp.Api.Interfaces.Services;
using Exerp.Api.Services;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using PureGym.Core.Interfaces;

namespace Exerp.Api.Tests.Services
{
    [TestFixture]
    public class CentreServiceCachedTests
    {
        private Mock<ICentreService> _centreService;
        private Mock<ICacheProvider> _cacheProvider;
        private const int InvalidCentreId = 705;
        private const int ValidCentreId = 701;


        [SetUp]
        public void Setup()
        {
            _centreService = new  Mock<ICentreService>();
            _cacheProvider = new  Mock<ICacheProvider>();

            _centreService.Setup(a => a.GetActiveCentres()).Returns(new Dictionary<int, string>() { { ValidCentreId, "London" } });
            _centreService.Setup(a => a.GetActiveCentresWithDetails()).Returns(new Dictionary<string, Centre>()
            {
                { "LONDON", new Centre() { CenterId = ValidCentreId,WebName ="London",Region = "London" } },
                 { "EDINBURGH", new Centre() { CenterId = ValidCentreId,WebName ="Edinburgh",Region = "Scotland"  } },
                  { "LONDON2", new Centre() { CenterId = ValidCentreId,WebName ="London2",Region = "London"  } }
            });

            _cacheProvider.SetupSequence(a => a.GetValue<Dictionary<string, Centre>>(It.IsAny<string>()))
                .Returns(null)
                .Returns(_centreService.Object.GetActiveCentresWithDetails());

            _cacheProvider.SetupSequence(a => a.GetValue<Dictionary<int, string>>(It.IsAny<string>()))
              .Returns(null)
              .Returns(_centreService.Object.GetActiveCentres());

        }

        [Test]
        public void When_Getting_Active_Centre_By_Id_If_Not_Found_Return_Null()
        {
            var subject = new CentreServiceCached(_centreService.Object,_cacheProvider.Object);
            var result = subject.GetActiveCentreById(InvalidCentreId);
            result.Should().BeNull();
        }

        [Test]
        public void When_Getting_Active_Centre_By_Id_If_Found_Return()
        {
            var subject = new CentreServiceCached(_centreService.Object,_cacheProvider.Object);
            var result = subject.GetActiveCentreById(ValidCentreId);
            result.Should().NotBeNull();
        }

        [TestCase("london")]
        [TestCase("loNdon")]
        [TestCase("LONDON")]
        public void When_Getting_Active_Centre_Name_Is_Not_Case_Sensitive(string name)
        {
            var subject = new CentreServiceCached(_centreService.Object,_cacheProvider.Object);
            var result = subject.GetActiveCentreByName(name);

            result.Should().NotBeNull();
            result.WebName.ShouldBeEquivalentTo("London");
        }

        [Test]
        public void When_Getting_Active_Centre_Name_Second_Call_Is_Cached()
        {
           var subject = new CentreServiceCached(_centreService.Object,_cacheProvider.Object);
 
           var result1 = subject.GetActiveCentreByName("London");
           var result2 = subject.GetActiveCentreByName("London");

            _cacheProvider.Verify(a => a.Add(It.IsAny<string>(), It.IsAny<Dictionary<string, Centre>>(), It.IsAny<TimeSpan>()), Times.Once());
        }
        
        [Test]
        public void When_Getting_Active_Regions_Regions_Are_Unique()
        {
            var subject = new CentreServiceCached(_centreService.Object,_cacheProvider.Object);
            var result = subject.GetActiveRegions().ToArray();

            result.Should().HaveCount(2);
            result.First().ShouldBeEquivalentTo("London");
            result.Last().ShouldBeEquivalentTo("Scotland");
        }

         [Test]
        public void When_Getting_Active_Centres_with_Details_Second_Call_Is_Cached()
        {
            var subject = new CentreServiceCached(_centreService.Object,_cacheProvider.Object);
            var result1 = subject.GetActiveCentresWithDetails();
            var result2 = subject.GetActiveCentresWithDetails();

            _cacheProvider.Verify(a => a.Add(It.IsAny<string>(), It.IsAny<Dictionary<string, Centre>>(),It.IsAny<TimeSpan>()), Times.Once());
        }
         
        [Test]
        public void When_Getting_Active_Centres_Second_Call_Is_Cached()
        {
           var subject = new CentreServiceCached(_centreService.Object,_cacheProvider.Object);
           var result1 = subject.GetActiveCentres();
           var result2 = subject.GetActiveCentres();

           _cacheProvider.Verify(a => a.Add(It.IsAny<string>(), It.IsAny<Dictionary<int, string>>(), It.IsAny<TimeSpan>()), Times.Once());
        }
    }
}