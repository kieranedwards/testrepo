using System;
using System.Collections.Generic;
using System.Linq;
using Exerp.Api.DataTransfer;
using Exerp.Api.Interfaces.Services;
using PureRide.Web.ApplicationServices.Credits;
using PureRide.Web.ViewModels.Account;

namespace PureRide.Web.ApplicationServices.Account
{
    public class AccountDashboardViewModelBuilder : IAccountDashboardViewModelBuilder 
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IPersonService _personService;
        private readonly ICreditBalanceService _creditBalanceService;

        public AccountDashboardViewModelBuilder(IAuthenticationService authenticationService, IPersonService personService, ICreditBalanceService creditBalanceService)
        {
            _authenticationService = authenticationService;
            _personService = personService;
            _creditBalanceService = creditBalanceService;
        }

        public AccountDashboardCoreModel Build()
        {
            var user = _authenticationService.GetCurrentUser();
            if (user == null)
                throw new ArgumentException("User must be logged in");

            var details = _personService.GetPersonDetails(user);

            var model = new AccountDashboardCoreModel
            {
                Name = string.Concat(details.FirstName," ",details.LastName),
                Email = details.Email,
                Phone = details.MobilePhoneNumber,
                ShoeSize = details.ShoeSize,
                Birthday = details.Birthday.ToShortDateString(),
                FriendsCount = details.Friends.Count()
            };

            return model;
        }

        public AccountDashboardBookingsModel BuildBookingsModel(bool includePast)
        {
            var user = _authenticationService.GetCurrentUser();
            if (user == null)
                throw new NotSupportedException("User must be logged in");


            IEnumerable<KeyValuePair<int, IEnumerable<Participation>>> bookings = _personService.GetFutureParticipations(user);

            if (includePast)
            {
              bookings  = bookings.Concat(_personService.GetPastParticipations(user)); 
            }
 
            return new AccountDashboardBookingsModel() { Bookings = bookings };
        }

        public IEnumerable<AccountDashboardCreditsModel> BuildCreditStatusModel()
        {
            var user = _authenticationService.GetCurrentUser();
            if (user == null)
                throw new NotSupportedException("User must be logged in");

            return _creditBalanceService.GetBalances().Select(a=> new AccountDashboardCreditsModel{Region=a.Key, Credits=a.Value});
        }
    }

    public interface IAccountDashboardViewModelBuilder
    {
        AccountDashboardCoreModel Build();
        AccountDashboardBookingsModel BuildBookingsModel(bool includePast);
        IEnumerable<AccountDashboardCreditsModel> BuildCreditStatusModel();
    }

   
}
