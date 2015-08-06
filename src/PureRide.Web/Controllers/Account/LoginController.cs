using System;
using System.Web.Mvc;
using PureRide.Web.ApplicationServices;
using PureRide.Web.ApplicationServices.Account;
using PureRide.Web.ViewModels.Account;
using Umbraco.Web;
using Umbraco.Web.Mvc;

namespace PureRide.Web.Controllers.Account
{
    public class LoginController  : SurfaceController 
{
        private readonly IAuthenticationService _authenticationService;
        
        public LoginController(IAuthenticationService authenticationService) : this(UmbracoContext.Current)
        {
            _authenticationService = authenticationService;
        }

        public LoginController(UmbracoContext umbracoContext)
            : base(umbracoContext)
        {
        }


        [ChildActionOnly]
        public ActionResult LoginForm()
        {
            var model = new AccountLoginModel();

            if (Request.QueryString["action"] != null)
            {
                _authenticationService.Logout();
                model.IsRecentLogOut = true;
            }

            return View(model);
        }
 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult FormSubmit(AccountLoginModel model, string returnUrl)
        {
            if (UserNameOrPasswordInvalid(model))
            {
                ModelState.AddModelError("Email", "The username or password entered was incorrect. Please check and try again.");
                return CurrentUmbracoPage();
            }

            return IsRedirect(returnUrl)
                ? (ActionResult) Redirect(returnUrl)
                : RedirectToUmbracoPage((int)WellKnownPage.AccountPublishedPage);
        }

        private bool IsRedirect(string returnUrl)
        {
            return Url.IsLocalUrl(returnUrl) && !String.IsNullOrWhiteSpace(returnUrl);
        }

        private bool UserNameOrPasswordInvalid(AccountLoginModel model)
        {
            return !(ModelState.IsValid && _authenticationService.LoginPerson(model));
        }
 
    }
}
