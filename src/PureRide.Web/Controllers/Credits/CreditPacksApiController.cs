using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using System.Web.Mvc;
using PureRide.Web.ApplicationServices.Credits;
using Umbraco.Web.Mvc;

namespace PureRide.Web.Controllers.Credits
{
    public class CreditPacksApiController : ApiController
    {
        private readonly ICreditPacksViewModelBuilder _creditPacksViewModelBuilder;
        private readonly ICreditPackBasketService _creditPackBasketService;

        public CreditPacksApiController(ICreditPacksViewModelBuilder creditPacksViewModelBuilder, 
                                    ICreditPackBasketService creditPackBasketService)
        {
            _creditPacksViewModelBuilder = creditPacksViewModelBuilder;
            _creditPackBasketService = creditPackBasketService;
        }

        [System.Web.Http.HttpPost]
        public ActionResult Select(string productId, string code = "")
        {
            var nextUrl = _creditPackBasketService.SetSelectedClipCard(productId,code);
            return new JsonNetResult() { Data = new{RedirectUrl=nextUrl}};
        }

        [System.Web.Http.HttpGet]
        public ActionResult CreditPacks(string location, string code = "")
        {
            var model = _creditPacksViewModelBuilder.BuildModel(location, code);
            return new JsonNetResult() { Data = model };
        }

        //This breaks the iframe and then redirects to the correct page
        [System.Web.Http.HttpGet]
        public HttpResponseMessage SagePostPack(string status, string txAuthNo)
        {
            var response = new HttpResponseMessage();
            var url = string.Compare(status, "ok", StringComparison.OrdinalIgnoreCase) == 0 ? string.Concat("/credits/thank-you/?txAuthNo=", txAuthNo) : string.Concat("/credits/payment-cancelled/?status=", status);
            response.Content = new StringContent(string.Format("<html><head><script>top.location.href = '{0}'</script></head><body></body></html>",url));
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("text/html");
            return response;
        }

    }
}
