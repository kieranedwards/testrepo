using SagePay.Core;

namespace SagePay.Interfaces
{
    public interface ITransactionClient
    {
        SagePayMessage Request(ITransactionRequestBuilder transactionRequestBuilder);
    }
}