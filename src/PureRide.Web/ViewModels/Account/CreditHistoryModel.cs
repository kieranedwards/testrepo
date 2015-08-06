using System.Collections.Generic;
using Exerp.Api.DataTransfer;

namespace PureRide.Web.ViewModels.Account
{

    public class CreditHistoryModel
    {
        public IEnumerable<PurchasedClipCard> ActiveClipCards { get; set; }
        public IEnumerable<PurchasedClipCard> InActiveClipCards { get; set; }
    }
}
