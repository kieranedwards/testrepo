using System.Collections.Generic;
using Exerp.Api.DataTransfer;

namespace Exerp.Api.Interfaces.Services
{
    public interface IClipCardPurchaseService
    {
        void PurchaseClipCard(PurchaseDetails purchaseDetails);
        
        //cachable
        IEnumerable<AvailableClipcard> GetAvailableClipPacks(Identity id, string campaignCode, string centreName);
        AvailableClipcard GetClipPack(Identity personId, Identity productId, string campaignCode);
        bool ValidateCampaignCode(string campaignCode, int centreId);
    }
}