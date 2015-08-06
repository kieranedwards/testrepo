using SagePay.Core;

namespace SagePay.Interfaces
{
    public interface IPaymentListenerUrlProvider
    {
        string GetReturnUrlForStatus(SagePayMessage.ResponseStatus inboundStatus, string txAuthNo);
        string GetNotificationUrl();
    }
}