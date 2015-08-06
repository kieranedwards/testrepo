namespace PureRide.Web.ViewModels.Credits
{
    public class CreditPackModel
    {
        public int Quantity { get; set; }
        public string Title { get; set; }
        public string Notes { get; set; }
        public decimal Price { get; set; }
        public string ProductId { get; set; }
        public bool IsSpecialOffer { get; set; }
    }
}
