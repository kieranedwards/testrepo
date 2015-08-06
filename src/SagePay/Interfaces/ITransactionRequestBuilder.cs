using System.Collections.Specialized;
using SagePay.InitalRequest;

namespace SagePay.Interfaces
{
    public interface ITransactionRequestBuilder
    {
        TransactionRequestRequestBuilder WithSettings(ISagePageSettings settings);
        TransactionRequestRequestBuilder WithAddress(IAddress address);
        TransactionRequestRequestBuilder WithAmount(decimal amount, string description);
        TransactionRequestRequestBuilder WithShippingAddress(IAddress address);
        TransactionRequestRequestBuilder WithBillingAddress(IAddress address);
        NameValueCollection AsParameterList();
        string SagePayEndPoint { get; }
    }
}