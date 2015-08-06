using System;
using Exerp.Api.DataTransfer;
using FluentAssertions;
using NUnit.Framework;

namespace Web.Tests.ViewModels
{
    [TestFixture]
    public class SelectedCreditPack
    {
        [Test]
        public void When_Create_CreditPack_Set_Values_Properties_Are_Set()
        {
            var subject = new PureRide.Web.ViewModels.SelectedCreditPack("1243:23354","TEST-123");
            subject.ProductId.ShouldBeEquivalentTo(new Identity("1243:23354"));
            subject.CampaignCode.ShouldBeEquivalentTo("TEST-123");
        }


        [Test]
        public void When_Create_CreditPack_Set_Values_And_Invalid_ProductId_Error()
        {
            Action action = () => new PureRide.Web.ViewModels.SelectedCreditPack("xxxxx:23354", "TEST-123");
            action.ShouldThrow<ArgumentException>();
        }

        
    }
}
