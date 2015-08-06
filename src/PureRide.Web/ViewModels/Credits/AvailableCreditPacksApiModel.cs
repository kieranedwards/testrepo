using System.Collections.Generic;

namespace PureRide.Web.ViewModels.Credits
{
    public class AvailableCreditPacksModel  
    {
        public IEnumerable<CreditPackModel> AvailableCreditPacks { get; set; }
        public string Location { get; set; }
        public string PromotionalCode{ get; set; }
        public bool IsPromotionalCodeValid { get; set; }
    }
}