using System;
using System.Web;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using PureRide.Web.Providers;
using SagePay.Core;
using SagePay.Interfaces;

namespace Web.Tests.Providers
{
    [TestFixture]
    public class PaymentListenerReturnUrlProviderTests
    {
        private Mock<IHttpContextProvider>_httpContextProvider;
        private Mock<ISagePageSettings> _sagePageSettings;

        [SetUp]
        public void Setup()
        {

            _httpContextProvider = new Mock<IHttpContextProvider>();

            var mockRequest = new Mock<HttpRequestBase>();
            mockRequest.Setup(a => a.Url).Returns(new Uri("https://testdomain.com/samplepage/?s=test"));
            
            var baseContext = new Mock<HttpContextBase>();
            baseContext.Setup(a => a.Request).Returns(mockRequest.Object);
            
            _httpContextProvider = new Mock<IHttpContextProvider>();
            _httpContextProvider.Setup(a => a.Current).Returns(baseContext.Object);

            _sagePageSettings= new Mock<ISagePageSettings>();
            _sagePageSettings.Setup(a => a.SagePayNotificationUrl).Returns("/notify/");
            _sagePageSettings.Setup(a => a.SagePayReturnUrl).Returns("/return/");
        }

        [Test]
        public void When_Build_Notification_Url_Contains_Current_Domain()
        {
            var subject = new PaymentListenerUrlProvider(_httpContextProvider.Object,_sagePageSettings.Object);
            var result = subject.GetNotificationUrl();
            result.Should().BeEquivalentTo("https://testdomain.com/notify/");
        }

        [Test]
        public void When_Build_Return_Url_Contains_Current_Domain()
        {
            var subject = new PaymentListenerUrlProvider(_httpContextProvider.Object, _sagePageSettings.Object);
            var result = subject.GetReturnUrlForStatus(SagePayMessage.ResponseStatus.OK,String.Empty);
            result.Should().StartWithEquivalent("https://testdomain.com/return/");
        }

        [Test]
        public void When_Build_Return__Url_Contains_Current_Status_QueryString()
        {
            var subject = new PaymentListenerUrlProvider(_httpContextProvider.Object, _sagePageSettings.Object);
            var result = subject.GetReturnUrlForStatus(SagePayMessage.ResponseStatus.OK,"123");
            result.Should().EndWithEquivalent("return/?status=OK&txAuthNo=123");
        }

    }
 
}
