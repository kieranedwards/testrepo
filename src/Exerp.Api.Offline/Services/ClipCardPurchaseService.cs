using System;
using System.Collections.Generic;
using System.Linq;
using Exerp.Api.DataTransfer;
using Exerp.Api.Offline.SampleData;
using Exerp.Api.Interfaces.Services;
using Exerp.Api.WebServices.SubscriptionAPI;

namespace Exerp.Api.Offline.Services
{
    public class ClipPackPurchaseService : IClipCardPurchaseService
    {
        private readonly ISampleDataProvider _sampleDataProvider;
        private readonly ICentreService _centreService;

        public ClipPackPurchaseService(ISampleDataProvider sampleDataProvider,ICentreService centreService)
        {
            _sampleDataProvider = sampleDataProvider;
            _centreService = centreService;
        }
         
        public IEnumerable<AvailableClipcard> GetAvailableClipPacks(Identity id, string campaignCode, string centreName)
        {
            var centre = _centreService.GetActiveCentreByName(centreName);

            if (centre==null)
                return Enumerable.Empty<AvailableClipcard>();

            return GetAvailableClipPacks(id, campaignCode, centre.CenterId);
        }

        private IEnumerable<AvailableClipcard> GetAvailableClipPacks(Identity id, string campaignCode, int centreId)
        {
           
            var discount = 0;
            if (!string.IsNullOrWhiteSpace(campaignCode))
                discount = GetDiscount(campaignCode);

            var centres = ((IEnumerable<dynamic>)_sampleDataProvider.GetDataSet().centres);
            var centre =
                centres.OrderBy(a => (int)a.id)
                    .First(a => (int)a.id == centreId);

            var packs = centre.clipPacks;

            var result = new List<AvailableClipcard>();
            foreach (var p in packs)
            {
                result.Add(new AvailableClipcard()
                {
                    ProductId = new Identity((int)centre.id, (int)p.id),
                    Name = p.name,
                    NormalPrice = p.normalPrice,
                    Price = ((decimal)p.price + (((decimal)p.price / 100) * discount)).ToString("###.00"),
                    Clips = (int)p.clips,
                    ValidPeriodLength = (int)p.validPeriodLength,
                    ValidPeriodLengthSpecified = p.validPeriodLengthSpecified,
                    ValidPeriodLengthUnit = (timeUnit)p.validPeriodLengthUnit,
                    ValidPeriodLengthUnitSpecified = p.validPeriodLengthUnitSpecified
                });
            }
            return result;
        }


        private int GetDiscount(string campaignCode)
        {
            if (string.IsNullOrEmpty(campaignCode))
                return 0;

            var codes = ((IEnumerable<dynamic>)_sampleDataProvider.GetDataSet().promotionCodes);
            return (int)codes.Single(a => string.Compare((string)a.code, campaignCode, StringComparison.OrdinalIgnoreCase) == 0).percentageDiscount;
        }

        public bool ValidateCampaignCode(string campaignCode, int centreId)
        {
            if (string.IsNullOrEmpty(campaignCode))
                return true;

            var  codes = ((IEnumerable<dynamic>)_sampleDataProvider.GetDataSet().promotionCodes);
            return codes.Any(a => string.Compare((string)a.code, campaignCode, StringComparison.OrdinalIgnoreCase) == 0);
        }

        public void PurchaseClipCard(PurchaseDetails purchaseDetails)
        {
            throw new System.NotImplementedException();
        }

        public AvailableClipcard GetClipPack(Identity personId, Identity productId, string campaignCode)
        {
            return GetAvailableClipPacks(personId, campaignCode, productId.CentreId).Single(a => Equals(a.ProductId.EntityId, productId.EntityId));
        }
    }
}
