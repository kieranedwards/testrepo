namespace PureRide.Web.ViewModels.Credits
{
    public class PaymentModel
    {
        public string Url { get; private set; }

        public PaymentModel(string url)
        {
            Url = url;
        }
    }
}
