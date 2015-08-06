using Exerp.Api.WebServices.SubscriptionAPI;
// ReSharper disable InconsistentNaming

namespace Exerp.Api.Interfaces.WebServices
{
    internal interface ISubscriptionApi
    {
        compositeKey sellClipcard(sellClipcardParameters parameters);
        availableClipcard[] getAvailableClipcardsForPerson(getAvailableClipcardsForPersonParameters parameters);
        getAvailableClipcardsForAnonymousResponse getAvailableClipcardsForAnonymous(getAvailableClipcardsForAnonymousParameters parameters);
        campaignInformation validateCampaignCode(getCampaignsInformationByCodeParameters parameters);

        /// <param name="personId"></param>
        /// <param name="finished">If true only finished clipcards returned, If false only non finished clipcards returned</param>
        /// <param name="canceled">Always set false</param>
        /// <param name="blocked">Always set false</param>
        /// <returns></returns>
        clipcard[] getPersonClipCards(compositeKey personId, bool finished, bool canceled, bool blocked);
    }
}
