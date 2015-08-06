using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SagePay.Interfaces;

namespace SagePay.InitalRequest
{
    public class TransactionRequestBuilderFactory : ITransactionRequestBuilderFactory
    {
        private readonly ITransactionParameterValidator _transactionParameterValidator;
        private readonly IPaymentListenerUrlProvider _paymentListenerUrlProvider;

        public TransactionRequestBuilderFactory(ITransactionParameterValidator transactionParameterValidator, IPaymentListenerUrlProvider paymentListenerUrlProvider)
        {
            _transactionParameterValidator = transactionParameterValidator;
            _paymentListenerUrlProvider = paymentListenerUrlProvider;
        }

        public ITransactionRequestBuilder CreateBuilder()
        {
            return new TransactionRequestRequestBuilder(_transactionParameterValidator,_paymentListenerUrlProvider);
        }
    }
}
