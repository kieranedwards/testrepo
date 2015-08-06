using System;
using AutoMapper;
using Exerp.Api.Interfaces.Services;
using PureRide.Web.ApplicationServices.Account;
using PureRide.Web.Exceptions;
using PureRide.Web.ViewModels.Credits;
using SagePay.Core;
using SagePay.Interfaces;

namespace PureRide.Web.ApplicationServices.Credits
{
    public class PaymentViewModelBuilder : IPaymentViewModelBuilder
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IPersonService _personService;
        private readonly ITransactionClient _transactionClient;
        private readonly ITransactionRequestBuilderFactory _transactionRequestBuilder;
        private readonly ICreditPackBasketService _creditPackBasketService;
        private readonly ICreditPackPurchaseService _creditPackPurchaseService;
        private readonly ISagePageSettings _sagePageSettings;

        public PaymentViewModelBuilder(
            IAuthenticationService authenticationService, 
            IPersonService personService,
            ITransactionClient transactionClient,
            ITransactionRequestBuilderFactory transactionRequestBuilder,
            ISagePageSettings sagePageSettings, 
            ICreditPackBasketService creditPackBasketService,
            ICreditPackPurchaseService creditPackPurchaseService 
            )
        {
            _authenticationService = authenticationService;
            _personService = personService;
            _transactionClient = transactionClient;
            _transactionRequestBuilder = transactionRequestBuilder;
            _sagePageSettings = sagePageSettings;
            _creditPackBasketService = creditPackBasketService;
            _creditPackPurchaseService = creditPackPurchaseService;
        }
 

        public PaymentModel BuildModel()
        {
            var user = _authenticationService.GetCurrentUser();

            if (user == null)
                throw new NotSupportedException("User must be logged in");

            var person = _personService.GetPersonDetails(user);
            var billingAddress = Mapper.Map<BillingAddressModel>(person.Address);
            billingAddress.FirstNames = person.FirstName;
            billingAddress.Surname = person.LastName;
            billingAddress.Country = "GB";//We only support UK cards

            var selectedPack = _creditPackBasketService.GetSelectedClipCard();

            var details = _transactionRequestBuilder.CreateBuilder()
                            .WithSettings(_sagePageSettings)
                            .WithAmount(selectedPack.SalePrice, selectedPack.ProductDescription)
                            .WithAddress(billingAddress);
            
            var response = _transactionClient.Request(details);
            if (response.Status != SagePayMessage.ResponseStatus.OK)
            {
                throw new SagePayInitalRequestException(response.Status, response.StatusDetail);    
            }

            _creditPackPurchaseService.SetOrderTransactionId(selectedPack.BasketRef, response.VpsTxId);
            return new PaymentModel(response.NextUrl);
        }
    }

    public interface IPaymentViewModelBuilder
    {
        PaymentModel BuildModel();
    }
}
