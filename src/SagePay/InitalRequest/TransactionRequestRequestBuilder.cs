using System;
using System.Collections.Specialized;
using SagePay.Helpers;
using SagePay.Interfaces;

namespace SagePay.InitalRequest
{
   
    public class TransactionRequestRequestBuilder : ITransactionRequestBuilder 
    {
        private readonly ITransactionParameterValidator _transactionParameterValidator;
        private readonly NameValueCollection _transactionDetails;
        private readonly IPaymentListenerUrlProvider _paymentListenerUrlProvider;
         
        public string SagePayEndPoint { get; private set; }

        public TransactionRequestRequestBuilder(ITransactionParameterValidator transactionParameterValidator, IPaymentListenerUrlProvider paymentListenerUrlProvider)
        {
            _transactionParameterValidator = transactionParameterValidator;
            _paymentListenerUrlProvider = paymentListenerUrlProvider;
            _transactionDetails = new NameValueCollection();
            SetDefaults();
        }

        public TransactionRequestRequestBuilder WithSettings(ISagePageSettings settings)
        {
            SagePayEndPoint = settings.SagePayEndPoint;
            _transactionDetails.Add("NotificationURL",  _paymentListenerUrlProvider.GetNotificationUrl());
            _transactionDetails.Add("Vendor", settings.SagePayVendorName);
            return this;
        }

        public TransactionRequestRequestBuilder WithAddress(IAddress address)
        {
            WithBillingAddress(address);
            WithShippingAddress(address);

            return this;
        }

        public TransactionRequestRequestBuilder WithAmount(decimal amount, string description)
        {
            if (amount > 1000)//Sage pay limit it 100000 however for saftey will limmit to 1k
                throw new ArgumentOutOfRangeException("amount",string.Format("amount specified {0} is too great",amount));

            if (amount <= 0.01m)
                throw new ArgumentOutOfRangeException("amount", string.Format("amount specified {0} is too small", amount));

            _transactionDetails.Add("Currency","GBP");
            _transactionDetails.Add("Amount", amount.ToString("###0.00"));
            _transactionDetails.Add("Description", description.Truncate(100));
            return this;
        }

        public TransactionRequestRequestBuilder WithShippingAddress(IAddress address)
        {
  
        _transactionDetails.Add("DeliveryAddress1", address.Address1);
        _transactionDetails.Add("DeliveryAddress2", address.Address2);
        _transactionDetails.Add("DeliveryCity", address.City);
        _transactionDetails.Add("DeliveryCountry", address.Country);
        _transactionDetails.Add("DeliveryPostCode", address.PostCode);
        _transactionDetails.Add("DeliveryFirstnames", address.FirstNames);
        _transactionDetails.Add("DeliverySurname", address.Surname);
            return this;
        }

        public TransactionRequestRequestBuilder WithBillingAddress(IAddress address)
        {
            
            _transactionDetails.Add("BillingAddress1", address.Address1);
            _transactionDetails.Add("BillingAddress2", address.Address2);
            _transactionDetails.Add("BillingCity",  address.City);
            _transactionDetails.Add("BillingCountry",  address.Country);
            _transactionDetails.Add("BillingPostCode",  address.PostCode);
            _transactionDetails.Add("BillingFirstnames",  address.FirstNames);
            _transactionDetails.Add("BillingSurname",  address.Surname);
            return this;
        }

        public NameValueCollection AsParameterList()
        {
            _transactionParameterValidator.Validate(_transactionDetails);
            return  _transactionDetails;
        }

        private void SetDefaults()
        {
            _transactionDetails.Add("VPSProtocol", "3.00");
            _transactionDetails.Add("PROFILE", "LOW");
            _transactionDetails.Add("TxType", "PAYMENT");
            _transactionDetails.Add("VendorTxCode", BuildTransactionCode());
        }

        private string BuildTransactionCode()
        {
            return Guid.NewGuid().ToString();
        }
    }
}
