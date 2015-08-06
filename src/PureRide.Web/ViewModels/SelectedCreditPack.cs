using Exerp.Api.DataTransfer;

namespace PureRide.Web.ViewModels
{
    public class SelectedCreditPack
    {
        public SelectedCreditPack(string productId, string campaignCode)
        {
            ProductId = new Identity(productId);
            CampaignCode = campaignCode;
        }

        public Identity ProductId { get; private set; }
        public string CampaignCode { get; private set; }
    }
}