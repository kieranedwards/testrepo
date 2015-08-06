namespace Exerp.Api.DataTransfer
{
    public class PurchaseDetails
    {
        public Identity PersonId { get; set; }
        public Identity ProductId { get; set; }
        public string CampaignCode { get; set; }
        public decimal AmountPaid { get; set; }
        public string SageTxAuthNo { get; set; }
    }
}
