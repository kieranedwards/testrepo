using System.Web.Security;
using Exerp.Api.DataTransfer;
using Exerp.Api.Interfaces.Services;
using PureRide.Web.Providers;
using PureRide.Web.ViewModels.Account;

namespace PureRide.Web.ApplicationServices.Account
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IHttpContextProvider _httpContextProvider;
        private readonly IPersonService _personService;

        public AuthenticationService(IHttpContextProvider httpContextProvider,IPersonService personService)
        {
            _httpContextProvider = httpContextProvider;
            _personService = personService;
        }

        public bool LoginPerson(Identity identity)
        {
            if (identity==null)
                return false;
            
            FormsAuthentication.SetAuthCookie(identity.ToString(), true);
            return true;
        }

        public bool LoginPerson(AccountLoginModel model)
        {
            var user = _personService.ValidateLoginByEmailAddress(model.Email, model.Password);
            return LoginPerson(user);
        }

        public bool ValidatePassword(Identity identity,string password)
        {
            return _personService.ValidateLoginById(identity, password)!=null;
        }

        public Identity GetCurrentUser()
        {
            if (_httpContextProvider.Current.User == null)
                return null;

            if (string.IsNullOrWhiteSpace(_httpContextProvider.Current.User.Identity.Name))
                return null;

            return new Identity(_httpContextProvider.Current.User.Identity.Name);
        }

        public void Logout()
        {
             FormsAuthentication.SignOut();
        }
    }

    public interface IAuthenticationService
    {
        bool LoginPerson(Identity identity);
        bool LoginPerson(AccountLoginModel model);
        bool ValidatePassword(Identity identity, string password);
        Identity GetCurrentUser();
        void Logout();
    }
}
