using Autofac;
using Exerp.Api.DataTransfer;
using Exerp.Api.Interfaces.Services;
using FluentAssertions;
using NUnit.Framework;

namespace Exerp.Api.Offline.Integration.Services
{
    [TestFixture]
    public class ClipPackPurchaseServiceTests{

        private IClipCardPurchaseService _clipCardPurchaseService;
        private int _centreId = 0;

        [TestFixtureSetUp]
        public void Init()
        {
            using (var scope = DependencyConfiguration.RegisterDependencies().BeginLifetimeScope())
            {
                _clipCardPurchaseService = scope.Resolve<IClipCardPurchaseService>();
            }
        }
       
        [Test]
        public void When_GetAvailableClipPacks_WithDefault()
        {
            var result = _clipCardPurchaseService.GetAvailableClipPacks(null, null, "River Street");
            result.Should().NotBeEmpty();
        }

        [Test]
        public void When_GetAvailableClipPacks_WithCampaignCode()
        {
            var result = _clipCardPurchaseService.GetAvailableClipPacks(null, "promo-10", "River Street");
            result.Should().NotBeEmpty();
        }

        [Test]
        public void When_GetAvailableClipPacks_WithInvalidLocation()
        {
            var result = _clipCardPurchaseService.GetAvailableClipPacks(null, null, "Invalid");
            result.Should().BeEmpty();
        }

        [Test]
        public void When_ValidateCampaignCode_ValidCode_True()
        {
            var result = _clipCardPurchaseService.ValidateCampaignCode("Promo-10", _centreId);
            result.Should().BeTrue(); 
        }

        [Test]
        public void When_ValidateCampaignCode_InValidCode_False()
        {
            var result = _clipCardPurchaseService.ValidateCampaignCode("Invalid", _centreId);
            result.Should().BeFalse();
        }

        [Test]
        public void When_ValidateCampaignCode_EmptyCode_True()
        {
            var result = _clipCardPurchaseService.ValidateCampaignCode("", _centreId);
            result.Should().BeTrue();
        }

        [Test]
        public void PurchaseClipCard(PurchaseDetails purchaseDetails)
        {
           
        }
    }
}
