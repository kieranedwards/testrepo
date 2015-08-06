using System.Collections.Generic;
using System.Linq;
using Exerp.Api.Interfaces.Services;
using PureRide.Web.ApplicationServices.Account;
using PureRide.Web.Helpers;
using PureRide.Web.ViewModels.Credits;

namespace PureRide.Web.ApplicationServices.Credits
{
    public class CreditPacksViewModelBuilder : ICreditPacksViewModelBuilder
    {
        private readonly IClipCardPurchaseService _clipCardPurchaseService;
        private readonly IAuthenticationService _authenticationService;
        private readonly ICentreService _centreService;
        private readonly ICreditPackModelAdapter _creditPackModelAdapter;

        public CreditPacksViewModelBuilder(IClipCardPurchaseService clipCardPurchaseService,
                                             IAuthenticationService authenticationService,
                                             ICentreService centreService,
                                             ICreditPackModelAdapter creditPackModelAdapter)
        {
            _clipCardPurchaseService = clipCardPurchaseService;
            _authenticationService = authenticationService;
            _centreService = centreService;
            _creditPackModelAdapter = creditPackModelAdapter;
        }

        public AvailableCreditPacksModel BuildModel(string location, string promotionalCode)
        {
            location = location.FromStudioSlug();

            var centre = _centreService.GetActiveCentreByName(location);

            if (centre == null)
                return null;

            var codeIsValid = _clipCardPurchaseService.ValidateCampaignCode(promotionalCode, centre.CenterId);
            var results = _clipCardPurchaseService.GetAvailableClipPacks(_authenticationService.GetCurrentUser(), codeIsValid ? promotionalCode : string.Empty, location);

            var availableCreditPacks = results.Select(a => _creditPackModelAdapter.Create(a, centre.Region)).ToList();
            return NewClipCardsModel(promotionalCode, availableCreditPacks, codeIsValid, centre.WebName);
        }

        private AvailableCreditPacksModel NewClipCardsModel(string promotionalCode, List<CreditPackModel> availableCreditPacks, bool codeIsValid, string locationName)
        {
            return new AvailableCreditPacksModel
            {   
                AvailableCreditPacks = availableCreditPacks,
                PromotionalCode = promotionalCode, 
                IsPromotionalCodeValid = codeIsValid,
                Location = locationName
            };
        }
 
    }
}