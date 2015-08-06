using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Exerp.Api.DataTransfer;
using Exerp.Api.Interfaces.Services;
using Exerp.Api.Interfaces.WebServices;
using Exerp.Api.WebServices.SubscriptionAPI;

namespace Exerp.Api.Services
{
    internal class ClipCardPurchaseService : IClipCardPurchaseService
    {
        private readonly ISubscriptionApi _subscriptionApi;
        private readonly ICentreService _centreService;

        public ClipCardPurchaseService(ISubscriptionApi subscriptionApi, ICentreService centreService)
        {
            _subscriptionApi = subscriptionApi;
            _centreService = centreService;
        }

        public IEnumerable<AvailableClipcard> GetAvailableClipPacks(Identity personId, string campaignCode,  string centreName)
        {
            var primaryCenter = _centreService.GetActiveCentreByName(centreName);

            if (!ValidateCampaignCode(campaignCode, primaryCenter.CenterId))
                campaignCode = string.Empty;

            availableClipcard[] apiResult;
            
            if (personId == null)
                apiResult = _subscriptionApi.getAvailableClipcardsForAnonymous(BuildClipCardSearchParameters(campaignCode, primaryCenter.CenterId)).clipcards;
            else
                apiResult = _subscriptionApi.getAvailableClipcardsForPerson(BuildClipCardSearchParameters(personId, campaignCode, primaryCenter.CenterId));
            
            return apiResult.Select(Mapper.Map<AvailableClipcard>);
        }

        public bool ValidateCampaignCode(string campaignCode,int centreId)
        {
            if (string.IsNullOrWhiteSpace(campaignCode))
                return true;

            return _subscriptionApi.validateCampaignCode(new getCampaignsInformationByCodeParameters(){campaignCode = campaignCode,center=centreId}).valid;
        }

        public void PurchaseClipCard(PurchaseDetails purchaseDetails)
        {
            _subscriptionApi.sellClipcard(Mapper.Map<sellClipcardParameters>(purchaseDetails));
        }

        public AvailableClipcard GetClipPack(Identity personId, Identity productId, string campaignCode)
        {
            if (personId == null)
                throw new ArgumentNullException("personId", "Person cannot be null");

           return _subscriptionApi.getAvailableClipcardsForPerson(BuildClipCardSearchParameters(personId, campaignCode,productId.CentreId))
                .Where(a => a.productId.id == productId.EntityId && a.productId.center == productId.CentreId)
                .Select(Mapper.Map<AvailableClipcard>)
                .FirstOrDefault();
        }

        private static getAvailableClipcardsForPersonParameters BuildClipCardSearchParameters(Identity personId, string campaignCode, int centerId)
        {
            return new getAvailableClipcardsForPersonParameters()
            {
                campaignCode = campaignCode,
                center = centerId,
                onlyAvailableOnWeb = false,
                personId = Mapper.Map<compositeKey>(personId)
            };
        }

        private getAvailableClipcardsForAnonymousParameters BuildClipCardSearchParameters(string campaignCode, int centerId)
        {
            return new getAvailableClipcardsForAnonymousParameters()
            {
                campaignCode = campaignCode,
                onlyAvailableOnWeb = false,
                centerId = centerId
            };
        }


    }
}
