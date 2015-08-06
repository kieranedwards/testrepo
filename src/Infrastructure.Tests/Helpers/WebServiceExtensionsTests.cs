using System.Net;
using Exerp.Api.Helpers;
using Exerp.Api.Interfaces.Configuration;
using Exerp.Api.Tests.Service_References;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace Exerp.Api.Tests.Helpers
{
    [TestFixture]
    public class WebServiceExtensionsTests
    {
        [Test]
        public void When_WithConfig_ProvidedDetails_SetOnTheService()
        {
            var loginDetails = new NetworkCredential("userName", "$password$");
            var config = new Mock<IExerpConfiguration>();
            config.Setup(a => a.ExerpApiBaseUrl).Returns("http://test.com/");
            config.Setup(a => a.ExerpApiCredentials).Returns(loginDetails);

            var service = (new TestResultService()).WithConfig(config.Object);

            service.Url.Should().BeEquivalentTo("http://test.com/TestResult");
            service.Credentials.Should().BeSameAs(loginDetails);
        }
    } 
    
  
}
