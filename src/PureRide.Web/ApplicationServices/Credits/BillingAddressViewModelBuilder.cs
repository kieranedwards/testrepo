using System;
using AutoMapper;
using Exerp.Api.DataTransfer;
using Exerp.Api.Interfaces.Services;
using PureRide.Web.ApplicationServices.Account;
using PureRide.Web.ViewModels.Credits;

namespace PureRide.Web.ApplicationServices.Credits
{
    public class BillingAddressViewModelBuilder : IBillingAddressViewModelBuilder
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IPersonService _personService;

        public BillingAddressViewModelBuilder(IAuthenticationService authenticationService,IPersonService personService)
        {
            _authenticationService = authenticationService;
            _personService = personService;
        }

        public BillingAddressModel BuildModel()
        {
            var user = _authenticationService.GetCurrentUser();

            if (user == null)
                throw new NotSupportedException("User must be logged in");

            var personAddress = _personService.GetPersonDetails(user).Address;
            return Mapper.Map<BillingAddressModel>(personAddress);
        }

        public void UpdateFromModel(BillingAddressModel model)
        {
            var user = _authenticationService.GetCurrentUser();

            if (user == null)
                throw new NotSupportedException("User must be logged in");

            _personService.UpdatePersonAddress(user,Mapper.Map<Address>(model));
        }
    }

    public interface IBillingAddressViewModelBuilder
    {
        BillingAddressModel BuildModel();
        void UpdateFromModel(BillingAddressModel model);
    }
}
