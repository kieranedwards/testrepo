using System;
using SagePay.Core;
using SagePay.Interfaces;

namespace PureRide.Web.Providers
{
    /// <summary>
    /// Builds URLs for the SagePay Payment Listener 
    /// </summary>
    public class PaymentListenerUrlProvider : IPaymentListenerUrlProvider
    {
        private readonly IHttpContextProvider _httpContextProvider;
        private readonly ISagePageSettings _sagePageSettings;

        public PaymentListenerUrlProvider(IHttpContextProvider httpContextProvider, ISagePageSettings sagePageSettings)
        {
            _httpContextProvider = httpContextProvider;
            _sagePageSettings = sagePageSettings;
        }

        public string GetReturnUrlForStatus(SagePayMessage.ResponseStatus inboundStatus, string txAuthNo)
        {
            var query = string.Concat("status=", inboundStatus.ToString(), "&txAuthNo=", txAuthNo);
            var url = GetBaseUrlBuilder(_sagePageSettings.SagePayReturnUrl,query);
            return url.ToString();
        }

        public string GetNotificationUrl()
        {
            var url = GetBaseUrlBuilder(_sagePageSettings.SagePayNotificationUrl,string.Empty);
            return url.ToString();
        }

        private UriBuilder GetBaseUrlBuilder(string newPath, string newQuery)
        {
             var url = new UriBuilder(_httpContextProvider.Current.Request.Url.ToString())
            {
                Path = newPath,
                Query = newQuery,
                Port = -1 //clear the port number
            };

            return url;
        }
    }
}
