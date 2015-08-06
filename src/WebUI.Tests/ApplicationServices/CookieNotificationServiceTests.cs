using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using PureRide.Web.ApplicationServices;
using PureRide.Web.Providers;

namespace Web.Tests.ApplicationServices
{
    [TestFixture]
    public class CookieNotificationServiceTests
    {
        private Mock<IHttpContextProvider> _httpContextProvider;
        private HttpCookieCollection _cookies;

        [SetUp]
        public void Setup()
        {   
            _cookies = new HttpCookieCollection();

            _httpContextProvider = new Mock<IHttpContextProvider>();

            var mockRequest = new Mock<HttpRequestBase>();
            mockRequest.Setup(a => a.IsSecureConnection).Returns(true);
            mockRequest.Setup(a => a.Cookies).Returns(_cookies);

            var mockResponse = new Mock<HttpResponseBase>();
            mockResponse.Setup(a => a.Cookies).Returns(_cookies);

            var baseContext = new Mock<HttpContextBase>();
            baseContext.Setup(a => a.Request).Returns(mockRequest.Object);
            baseContext.Setup(a => a.Response).Returns(mockResponse.Object);
      
            _httpContextProvider.Setup(a => a.Current).Returns(baseContext.Object);
        }

        [Test]
        public void When_Set_Cookie_Added_To_Response()
        {
          var subject = new CookieNotificationService(_httpContextProvider.Object);
          subject.SetUserCookie();

          _cookies.Should().HaveCount(1);
            // ReSharper disable once PossibleNullReferenceException
          _cookies[0].HttpOnly.Should().BeTrue();
          _cookies[0].Shareable.Should().BeFalse();
        }

        [Test]
        public void When_Set_Cookie_Under_Sll_Marked_Secure()
        {
            var subject = new CookieNotificationService(_httpContextProvider.Object);
            subject.SetUserCookie();

            _cookies.Should().HaveCount(1);
            // ReSharper disable once PossibleNullReferenceException
            _cookies[0].Secure.Should().BeTrue();
        }
  
        [Test]
        public void When_Checking_Cookie_And_Exists_Return_True()
        {
            _cookies.Add(new HttpCookie("CookieNotification"));
            var subject = new CookieNotificationService(_httpContextProvider.Object);
            var result = subject.UserHasViewedCookie();
            result.Should().BeTrue();
        }

        [Test]
        public void When_Checking_Cookie_And_Not_Exists_Return_False()
        {
            var subject = new CookieNotificationService(_httpContextProvider.Object);
            var result = subject.UserHasViewedCookie();
            result.Should().BeFalse();
        }

    }
}
