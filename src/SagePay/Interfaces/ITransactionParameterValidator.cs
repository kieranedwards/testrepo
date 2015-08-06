
using System.Collections.Specialized;

namespace SagePay.Interfaces
{
    public interface ITransactionParameterValidator
    {
        void Validate(NameValueCollection transactionDetails);
    }
}