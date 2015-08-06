using Exerp.Api.DataTransfer;
using PureRide.Web.ViewModels.Credits;

namespace PureRide.Web.ApplicationServices.Credits
{
    public class CreditPackModelAdapter : ICreditPackModelAdapter
    {
        public CreditPackModel Create(AvailableClipcard source, string regionName)
        {

            var durationSuffix = source.ValidPeriodLength > 1 ? "s" : "";

            var result = new CreditPackModel
            {
                IsSpecialOffer = source.Name.ToLower().Contains("offer"),
                Title = string.Format("{0} - £{1:c}", source.Name, source.Price),
                ProductId = source.ProductId.ToString(),
                Notes = BuildNoteFromSource(source, regionName, durationSuffix),
                Quantity = source.Clips 
            };

            if (PriceHasBeenDiscounted(source))
                result.Notes += string.Format("Price before discount £{0:c}", source.NormalPrice);

            return result;
        }

        private string BuildNoteFromSource(AvailableClipcard source, string regionName, string durationSuffix)
        {
            return string.Format("Valid for all {0} studios for {1} {2}{3} from purchase. ", regionName, source.ValidPeriodLength, source.ValidPeriodLengthUnit.ToString().ToLower(), durationSuffix);
        }

        private bool PriceHasBeenDiscounted(AvailableClipcard source)
        {
            return source.NormalPrice != null && source.Price != source.NormalPrice;
        }
    }
}