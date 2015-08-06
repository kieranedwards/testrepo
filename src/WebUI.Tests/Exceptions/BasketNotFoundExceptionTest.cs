using FluentAssertions;
using NUnit.Framework;
using PureRide.Web.Exceptions;

namespace Web.Tests.Exceptions
{
    [TestFixture]
    public class BasketNotFoundExceptionTest
    {
        [Test]
        public void When_Creating_Error_Detail_Are_Included()
        {
            var subject = new BasketNotFoundException("VX1234");
            var result = subject.Message;
            result.Should().ContainEquivalentOf("VX1234");
        }

    }
}
