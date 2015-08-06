using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Exerp.Api.DataTransfer;
using Exerp.Api.Interfaces.Services;
using Exerp.Api.Interfaces.WebServices;
using Exerp.Api.Services;
using Exerp.Api.WebServices.SubscriptionAPI;
using NUnit.Framework;
using FluentAssertions;
using Moq;
 

namespace Exerp.Api.Tests.Services
{
    [TestFixture]
    public class ClipCardPurchaseServiceTests
    {
        private const int ValidCentreId = 701;
        private const string ValidCampaignCode = "test-10";
       
        private  Mock<ICentreService> _centreService;
        private  Mock<ISubscriptionApi> _subscriptionApi;
        private readonly Identity _validPersonId = new Identity(ValidCentreId, 2);
        private readonly Identity _validProductId = new Identity(ValidCentreId, 3);
        

        [SetUp]
        public void SetUp()
        {
            _centreService = new Mock<ICentreService>();
            _centreService.Setup(a => a.GetActiveCentreByName("London")).Returns(new Centre() { CenterId = ValidCentreId });

            _subscriptionApi =  new Mock<ISubscriptionApi>();
        }

        [Test]
        public void When_Getting_ClipPack_For_User_Return_Single()
        {
            _subscriptionApi.Setup(
                a => a.getAvailableClipcardsForPerson(It.IsAny<getAvailableClipcardsForPersonParameters>()))
                .Returns(new[] {new availableClipcard(){name="1 Credit",productId = new compositeKey(){center =_validProductId.CentreId,id=_validProductId.EntityId}} } );
            
            var subject = new ClipCardPurchaseService(_subscriptionApi.Object, _centreService.Object);
            var result = subject.GetClipPack(_validPersonId, _validProductId, string.Empty);

            result.Should().NotBeNull();
            result.Name.ShouldBeEquivalentTo("1 Credit");
        }

        [Test]
        public void When_Getting_ClipPack_For_User_Not_LogedIn_Error()
        {
            _subscriptionApi.Setup(
                a => a.getAvailableClipcardsForPerson(It.IsAny<getAvailableClipcardsForPersonParameters>()))
                .Returns(new[] { new availableClipcard() { name = "1 Credit", productId = new compositeKey() { center = _validProductId.CentreId, id = _validProductId.EntityId } } });

            var subject = new ClipCardPurchaseService(_subscriptionApi.Object, _centreService.Object);
      
            Action result = () => subject.GetClipPack(null, _validProductId, string.Empty);
            result.ShouldThrow<ArgumentException>();          
        }

        [Test]
        public void When_Getting_Available_ClipPacks_For_User_Not_LogedIn_Call_Api()
        {
            getAvailableClipcardsForAnonymousParameters paramsIn = null;
            _subscriptionApi.Setup(
                a => a.getAvailableClipcardsForAnonymous(It.IsAny<getAvailableClipcardsForAnonymousParameters>()))
                .Callback<getAvailableClipcardsForAnonymousParameters>(p => paramsIn = p)
                .Returns(new getAvailableClipcardsForAnonymousResponse(){clipcards = new availableClipcard[]{}});

            _subscriptionApi.Setup(a => a.validateCampaignCode(It.IsAny<getCampaignsInformationByCodeParameters>()))
                .Returns(new campaignInformation() { valid = true });

            var subject = new ClipCardPurchaseService(_subscriptionApi.Object, _centreService.Object);
            subject.GetAvailableClipPacks(null, ValidCampaignCode, "London");

            _subscriptionApi.Verify(a => a.getAvailableClipcardsForAnonymous(It.IsAny<getAvailableClipcardsForAnonymousParameters>()));
            paramsIn.campaignCode.ShouldBeEquivalentTo(ValidCampaignCode);
            paramsIn.centerId.ShouldBeEquivalentTo(ValidCentreId);
            paramsIn.onlyAvailableOnWeb.Should().BeFalse();
         }

        [Test]
        public void When_Getting_Available_ClipPacks_Clear_Code_If_Invalid()
        {
            getAvailableClipcardsForAnonymousParameters paramsIn = null;
            _subscriptionApi.Setup(
                a => a.getAvailableClipcardsForAnonymous(It.IsAny<getAvailableClipcardsForAnonymousParameters>()))
                .Callback<getAvailableClipcardsForAnonymousParameters>(p => paramsIn = p)
                .Returns(new getAvailableClipcardsForAnonymousResponse() { clipcards = new availableClipcard[] { } });

            _subscriptionApi.Setup(a => a.validateCampaignCode(It.IsAny<getCampaignsInformationByCodeParameters>()))
                .Returns(new campaignInformation() { valid = false });

            var subject = new ClipCardPurchaseService(_subscriptionApi.Object, _centreService.Object);
            subject.GetAvailableClipPacks(null, "xyz", "London");

            _subscriptionApi.Verify(a => a.getAvailableClipcardsForAnonymous(It.IsAny<getAvailableClipcardsForAnonymousParameters>()));
            paramsIn.campaignCode.ShouldBeEquivalentTo(string.Empty);
            paramsIn.centerId.ShouldBeEquivalentTo(ValidCentreId);
            paramsIn.onlyAvailableOnWeb.Should().BeFalse();
        }

        [Test]
        public void When_Getting_Available_ClipPacks_For_User_LogedIn_Call_Api()
        {
            getAvailableClipcardsForPersonParameters paramsIn = null;
            _subscriptionApi.Setup(
                a => a.getAvailableClipcardsForPerson(It.IsAny<getAvailableClipcardsForPersonParameters>()))
                .Callback<getAvailableClipcardsForPersonParameters>(p => paramsIn = p)
                .Returns(new availableClipcard[] { });

            _subscriptionApi.Setup(a => a.validateCampaignCode(It.IsAny<getCampaignsInformationByCodeParameters>()))
                .Returns(new campaignInformation() { valid = true });

            var subject = new ClipCardPurchaseService(_subscriptionApi.Object, _centreService.Object);
            subject.GetAvailableClipPacks(_validPersonId, ValidCampaignCode, "London");

            _subscriptionApi.Verify(a => a.getAvailableClipcardsForPerson(It.IsAny<getAvailableClipcardsForPersonParameters>()));
            paramsIn.personId.id.ShouldBeEquivalentTo(_validPersonId.EntityId);
            paramsIn.campaignCode.ShouldBeEquivalentTo(ValidCampaignCode);
            paramsIn.center.ShouldBeEquivalentTo(ValidCentreId);
            paramsIn.onlyAvailableOnWeb.Should().BeFalse();
        }

        [Test]
        public void When_Purchase_Call_Api()
        {
            var details = new PurchaseDetails()
            {
                AmountPaid = 1.34m,
                CampaignCode = "test-10",
                PersonId = _validPersonId,
                ProductId = _validProductId,
                SageTxAuthNo = "VX-123"
            };

            sellClipcardParameters paramsIn = null;
            _subscriptionApi.Setup(a => a.sellClipcard(It.IsAny<sellClipcardParameters>()))
                .Callback<sellClipcardParameters>(p => paramsIn = p);
              
            var subject = new ClipCardPurchaseService(_subscriptionApi.Object, _centreService.Object);
            subject.PurchaseClipCard(details);

            _subscriptionApi.Verify(a => a.sellClipcard(It.IsAny<sellClipcardParameters>()));
            paramsIn.personId.id.ShouldBeEquivalentTo(_validPersonId.EntityId);
            paramsIn.productId.id.ShouldBeEquivalentTo(details.ProductId.EntityId);
            paramsIn.paymentInfo.creditCardTransactionRef.ShouldBeEquivalentTo(details.SageTxAuthNo);
            paramsIn.campaignCode.ShouldBeEquivalentTo(details.CampaignCode);
            paramsIn.paymentInfo.amountPaidByCustomer.ShouldBeEquivalentTo(details.AmountPaid.ToString("##.##"));
        }

         [Test]
        public void When_Validate_CampaignCode_Call_Api()
         {
            string validCode = "Test-10";
            string codePassedIn = string.Empty;
             
            _subscriptionApi.Setup(a => a.validateCampaignCode(It.IsAny<getCampaignsInformationByCodeParameters>()))
                .Callback<getCampaignsInformationByCodeParameters>(p => { codePassedIn = p.campaignCode; })
                .Returns(new campaignInformation(){valid=true});

            var subject = new ClipCardPurchaseService(_subscriptionApi.Object,_centreService.Object);
            subject.ValidateCampaignCode(validCode, ValidCentreId);

             _subscriptionApi.Verify(a=>a.validateCampaignCode(It.IsAny<getCampaignsInformationByCodeParameters>()));
             codePassedIn.ShouldAllBeEquivalentTo(validCode);
        }
             
        [Test]
        public void When_Validate_CampaignCode_With_a_Valid_Code_Return_True()
        {
             string ValidCode = "Test-10";

            _subscriptionApi.Setup(a => a.validateCampaignCode(It.IsAny<getCampaignsInformationByCodeParameters>()))
                .Returns(new campaignInformation(){valid=true});

            var subject = new ClipCardPurchaseService(_subscriptionApi.Object,_centreService.Object);
            var result = subject.ValidateCampaignCode(ValidCode, ValidCentreId);

            result.Should().BeTrue();
        }
        
        [Test]
        public void When_Validate_CampaignCode_With_a_Null_Code_Return_True()
        {
            var subject = new ClipCardPurchaseService(_subscriptionApi.Object,_centreService.Object);
            var result = subject.ValidateCampaignCode(null, ValidCentreId);
            result.Should().BeTrue();
        }

        
    }
}
