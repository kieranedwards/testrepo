using System;
using Exerp.Api.Interfaces.Services;
using PureRide.Web.ViewModels.Account;

namespace PureRide.Web.ApplicationServices.Account
{
    public class CreditHistoryViewModelBuilder : ICreditHistoryViewModelBuilder
    {
        private readonly IPersonService _personService;
        private readonly IAuthenticationService _authenticationService;

        public CreditHistoryViewModelBuilder(IPersonService personService, IAuthenticationService authenticationService)
        {
            _personService = personService;
            _authenticationService = authenticationService;
        }

        public CreditHistoryModel Build()
        {
            var user = _authenticationService.GetCurrentUser();
            if (user == null)
                throw new NotSupportedException("User must be logged in");
            
            return new CreditHistoryModel()
            {
                ActiveClipCards= _personService.GetActiveClipCards(user),
                InActiveClipCards= _personService.GetInActiveClipCards(user)
            };
        }
    }

    public interface ICreditHistoryViewModelBuilder
    {
        CreditHistoryModel Build();
    }
}
