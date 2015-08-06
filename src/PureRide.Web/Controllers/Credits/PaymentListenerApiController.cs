using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using PureRide.Web.ApplicationServices.Credits;

namespace PureRide.Web.Controllers.Credits
{

    /// <summary>
    /// This handles the sage page call back, could be seperate app in the future
    /// </summary>
    public class PaymentListenerApiController : ApiController
    {
        private readonly ICreditPackPurchaseService _creditPackPurchaseService;

        public PaymentListenerApiController(ICreditPackPurchaseService creditPackPurchaseService)
        {
            _creditPackPurchaseService = creditPackPurchaseService;
        }

        public HttpResponseMessage Index()
        {
            var bodyString = Request.Content.ReadAsStringAsync().Result;
            var result = _creditPackPurchaseService.ProcessPayment(bodyString);

            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(result.AsFormattedResponse(), Encoding.UTF8, "text/plain")
            };
        }
    }
}
