using Exerp.Api.DataTransfer;
using PureRide.Web.ViewModels.Credits;

namespace PureRide.Web.ApplicationServices.Credits
{
    public interface ICreditPackModelAdapter
    {
        CreditPackModel Create(AvailableClipcard source, string regionName);
    }
}