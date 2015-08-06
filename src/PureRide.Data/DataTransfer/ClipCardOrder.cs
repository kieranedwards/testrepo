using System;

namespace PureRide.Data.DataTransfer
{
    public class ClipCardOrder
    {
        public decimal SalePrice { get; set; }
        public string ProductDescription { get; set; }
        public Guid BasketRef { get; set; }
        public string CampaignCode { get; set; }
        public int ProductEntityId { get; set; }
        public int PersonEntityId { get; set; }
        public int PersonCentreId { get; set; }
        public int ProductCentreId { get; set; }
    }
}