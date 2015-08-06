using FluentAssertions;
using NUnit.Framework;
using PureRide.Web.Exceptions;
using SagePay.Core;

namespace Web.Tests.Exceptions
{
    [TestFixture]
    public class SagePayInitalRequestExceptionTests
    {
        [Test]
        public void When_Creating_Error_Code_And_Detail_Are_Included()
        {
            var subject = new SagePayInitalRequestException(SagePayMessage.ResponseStatus.ABORT, "Detail Message");
            var result = subject.Message;
            result.Should().ContainEquivalentOf("ABORT");
            result.Should().ContainEquivalentOf("Detail Message");
        }

    }
}
