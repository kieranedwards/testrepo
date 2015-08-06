using PureRide.Web.ViewModels.Credits;

namespace PureRide.Web.ApplicationServices.Credits
{
    public interface ICreditPacksViewModelBuilder
    {
        AvailableCreditPacksModel BuildModel(string location, string promotionalCode);
    }
}