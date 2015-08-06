using System;
using System.Collections.Generic;
using System.Linq;
using Exerp.Api.Interfaces.Services;
using PureRide.Web.ApplicationServices.Account;

namespace PureRide.Web.ApplicationServices.Credits
{
    public interface ICreditBalanceService
    {
        int GetBalance(string regionName);
        Dictionary<string,int> GetBalances();
    }

    public class CreditBalanceService : ICreditBalanceService
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IPersonService _personService;

        public CreditBalanceService(IAuthenticationService authenticationService, IPersonService personService)
        {
            _authenticationService = authenticationService;
            _personService = personService;
        }

        public int GetBalance(string regionName)
        {
            var user = _authenticationService.GetCurrentUser();

            if (user == null)
                return 0;

            var credits = _personService.GetActiveClipCards(user);

            return credits
                .Where(a => string.Compare(a.Region , regionName, StringComparison.OrdinalIgnoreCase) == 0)
                .Sum(a => a.ClipsLeft);
        }

        public Dictionary<string,int> GetBalances()
        {
            var user = _authenticationService.GetCurrentUser();

            if (user == null)
                return new Dictionary<string, int>();

            var credits = _personService.GetActiveClipCards(user);
            return credits.GroupBy(a=>a.Region).ToDictionary(b=>b.Key,c=>c.Sum(d=>d.ClipsLeft));
        }

    }
}
