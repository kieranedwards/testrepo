using System;
using System.Linq;
using System.Net;
using Exerp.Api.Helpers;
using Exerp.Api.Interfaces.Configuration;
using Exerp.Api.Interfaces.WebServices;
using Exerp.Api.Services;
using Exerp.Api.WebServices.PersonAPI;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace Exerp.Api.Tests.Services
{
    [TestFixture]
    public class CentreServiceTests
    {
        private Mock<IExerpConfiguration> _config;
        private Mock<IPersonApi> _personApi;
        private const int InactiveCentreId = 705;
        private const string InactiveCentreName = "Inactive";
        
        [SetUp]
        public void Setup()
        {
            var ExerpPureRideScopeId = 0;
            _config = new Mock<IExerpConfiguration>();
            _config.Setup(a => a.ExerpPureRidePrimaryCentreId).Returns(701);
            _config.Setup(a => a.ExerpPureRideScopeId).Returns(ExerpPureRideScopeId);

            _personApi = new Mock<IPersonApi>();
            _personApi.Setup(a => a.getDetailForCenters(string.Empty))
                .Returns(new[] {new centerDetail()
                {
                    webName = InactiveCentreName,
                    centerId = InactiveCentreId,
                    startupDate = DateTime.Today.AddDays(5).ToApiDate()
                }, 
                new centerDetail(){
                    centerId = 701,
                    startupDate = DateTime.Today.AddDays(-5).ToApiDate(),
                    webName ="London"
                }, new centerDetail(){
                    centerId = 702,
                    startupDate = DateTime.Today.AddDays(-5).ToApiDate(),
                    webName ="London2"
                }, new centerDetail(){
                    centerId = 709,
                    startupDate = DateTime.Today.AddDays(-5).ToApiDate(),
                    webName ="Edinburgh"
                }}).Verifiable();

            _personApi.Setup(a => a.getScope(scopeType.Area, ExerpPureRideScopeId)).Returns(
                new scope() { children = new[]
                {
                    new scope() { name = "United Kingdom", children = new[] { new scope() { name = "London", children = new[] { new scope(){name="London", id=701}, new scope(){name=InactiveCentreName, id=InactiveCentreId}, new scope(){name="London2", id=702}} }, new scope() { name = "Scotland", children = new[] { new scope(){name="Edinburgh", id=709}} }} }
                } 
                }
                );
        }

        [Test]
        public void When_Getting_Active_Centre_By_Id_Throw_Error()
        {
            var subject = new CentreService(_personApi.Object, _config.Object);
            
            Action result = () => subject.GetActiveCentreById(702);
            result.ShouldThrow<NotSupportedException>();     
        }

        [Test]
        public void When_Getting_Active_Centre_Ivalid_CentreId_Throw_Error()
        {
            _personApi.Setup(a => a.getScope(scopeType.Area, _config.Object.ExerpPureRideScopeId)).Returns(
               new scope()
               {
                   children = new[]
                {
                    new scope() { name = "United Kingdom", children = new[] { new scope() { name = "London", children = new[] { new scope(){name="London", id=100}} }} }
                }
               }
               );

            var subject = new CentreService(_personApi.Object, _config.Object);

            Action result = () => subject.GetActiveCentreByName("London");
            result.ShouldThrow<ArgumentOutOfRangeException>();
        }

        [Test]
        public void When_Getting_Active_Centre_Name_And_Not_Yet_Open_Return_Null()
        {
            var subject = new CentreService(_personApi.Object, _config.Object);
            var result = subject.GetActiveCentreByName(InactiveCentreName);

            result.Should().BeNull();
        }
        
        [Test]
        public void When_Getting_Active_Centre_Details_Populate_Regions()
        {
            var subject = new CentreService(_personApi.Object, _config.Object);
            var result = subject.GetActiveCentresWithDetails();

            result.Should().NotBeEmpty();
            result.Should().HaveCount(3);
            result.First().Value.Region.ShouldBeEquivalentTo("London");
            result.Last().Value.Region.ShouldBeEquivalentTo("Scotland");
        }

        [Test]
        public void When_Getting_Active_Centre_Name_And_Not_Found_Return_Null()
        {
            var subject = new CentreService(_personApi.Object, _config.Object);
            var result = subject.GetActiveCentreByName("xyz");

            result.Should().BeNull();
        }

        [TestCase("london")]
        [TestCase("loNdon")]
        [TestCase("LONDON")]
        public void When_Getting_Active_Centre_Name_Is_Not_Case_Sensitive(string name)
        {
            var subject = new CentreService(_personApi.Object, _config.Object);
            var result = subject.GetActiveCentreByName(name);

            result.Should().NotBeNull();
            result.WebName.ShouldBeEquivalentTo("London");
        }

        [Test]
        public void When_Getting_Active_Regions_Regions_Are_Unique()
        {
            var subject = new CentreService(_personApi.Object, _config.Object);
            var result = subject.GetActiveRegions().ToArray();

            result.Should().HaveCount(2);
            result.First().ShouldBeEquivalentTo("London");
            result.Last().ShouldBeEquivalentTo("Scotland");
        }

        [Test]
        public void When_Getting_Active_Regions_Calls_Api()
        {
            var subject = new CentreService(_personApi.Object, _config.Object);
            subject.GetActiveRegions();

            _personApi.Verify(a => a.getDetailForCenters(string.Empty));
        }

        [Test]
        public void When_Getting_Active_Centres_Calls_Api()
        {
            var subject = new CentreService(_personApi.Object, _config.Object);
            subject.GetActiveCentres();

            _personApi.Verify(a=>a.getDetailForCenters(string.Empty));
        }

        [Test]
        public void When_Getting_Active_Centres_Future_Openings_Are_Ignored()
        {
            var subject = new CentreService(_personApi.Object, _config.Object);
            var result = subject.GetActiveCentres();

            result.Should().NotContainKey(InactiveCentreId);//valid break must fix this issue
        }
 
    }
}
