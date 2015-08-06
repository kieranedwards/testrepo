using System;
using Exerp.Api.Interfaces.Services;
using PureRide.Web.ViewModels.Account;

namespace PureRide.Web.ApplicationServices.Account
{
    public class PasswordManagementService : IPasswordManagementService
    {
        private readonly IPersonService _personService;
        private readonly IAuthenticationService _authenticationService;

        public PasswordManagementService(IPersonService personService, IAuthenticationService authenticationService)
        {
            _personService = personService;
            _authenticationService = authenticationService;
        }

        public bool SendPasswordResetToken(string emailAddress)
        {
            return _personService.RequestPersonPasswordReset(emailAddress);
        }

        public bool UpdatePassword(UpdatePasswordModel model)
        {
            var currentUser = _authenticationService.GetCurrentUser();

            if (!_authenticationService.ValidatePassword(currentUser, model.CurrentPassword))
                return false;

            _personService.UpdatePersonPassword(currentUser, model.CurrentPassword, model.NewPassword);

            return true;
        }
    }

    public interface IPasswordManagementService
    {
        bool SendPasswordResetToken(string emailAddress);
        bool UpdatePassword(UpdatePasswordModel model);
    }
}
