using System.Collections.Generic;
using System.Collections.Specialized;
using SagePay.Exceptions;
using SagePay.Interfaces;

namespace SagePay.InitalRequest
{
    internal class TransactionParameterValidator : ITransactionParameterValidator
    {
        //Feild,MaxLength
        private readonly Dictionary<string, int> _requiredFields = new Dictionary<string, int>
        {
                {"Vendor", 40},
                {"Amount", 9},
                {"VendorTxCode", 40},
                {"Description", 100},
                {"Currency", 3},
                {"NotificationURL", 225},
                {"BillingFirstnames", Constants.NameMaxLength},
                {"BillingSurname", Constants.NameMaxLength},
                {"BillingAddress1", Constants.AddressLineMaxLength},
                {"BillingCity", Constants.CityMaxLength},
                {"BillingPostCode", Constants.PostCodeMaxLength},
                {"BillingCountry", Constants.CountryMaxLength},
                {"DeliveryFirstnames", Constants.NameMaxLength},
                {"DeliverySurname", Constants.NameMaxLength},
                {"DeliveryAddress1", Constants.AddressLineMaxLength},
                {"DeliveryCity", Constants.CityMaxLength},
                {"DeliveryPostcode", Constants.PostCodeMaxLength},
                {"DeliveryCountry", Constants.CountryMaxLength},
            }; 

        public void Validate(NameValueCollection transactionDetails)
        {
            foreach (var field in _requiredFields)
            {
                var value = transactionDetails[field.Key];
                if (string.IsNullOrWhiteSpace(value))
                    throw new SagePayFieldMissingException(string.Concat(field.Key," Field must be set"));
                
                if (value.Length > field.Value)
                    throw new SagePayFieldLengthException(string.Concat(field.Key, " Field must be set shorted in length than ", field.Value));
            }
        }
    }
}
