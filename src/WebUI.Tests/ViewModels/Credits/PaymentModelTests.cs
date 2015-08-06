using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using PureRide.Web.ViewModels.Credits;

namespace Web.Tests.ViewModels.Credits
{
    [TestFixture]
    public class PaymentModelTests
    {

        [Test]
        public void When_PaymentModel_Created_Set_Url()
        {
            var subject = new PaymentModel("http://test.com");
            subject.Url.ShouldBeEquivalentTo("http://test.com");
        }

    }
}
