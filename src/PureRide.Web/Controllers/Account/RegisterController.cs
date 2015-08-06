using System.Collections.Specialized;
using System.Web.Mvc;
using Exerp.Api.DataTransfer;
using PureRide.Web.ApplicationServices.Account;
using PureRide.Web.Helpers;
using PureRide.Web.ViewModels.Account;
using Umbraco.Web;
using Umbraco.Web.Mvc;

namespace PureRide.Web.Controllers.Account
{ 
        public class RegisterController  : SurfaceController 
{
            private readonly IPersonRegistrationService _personRegistrationService;

            public RegisterController(IPersonRegistrationService personRegistrationService)
                : this(UmbracoContext.Current)
            {
                _personRegistrationService = personRegistrationService;
            }

            public RegisterController(UmbracoContext umbracoContext)
            : base(umbracoContext)
        {
        }

        public ActionResult Index()
        {
            return View();
        }


        [ChildActionOnly]
        public ActionResult PreRegisterForm()
        {
            return View(new AccountEmailModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PreRegisterFormSubmit(AccountEmailModel model)
        {
            if (!ModelState.IsValid)
                return CurrentUmbracoPage().WithModel(TempData, model);

            var isNewAccount = _personRegistrationService.PreRegisterPerson(model);

            if (!isNewAccount)
            {
                ModelState.AddModelError("email", "We have a record of this email address. Please check your emails to reset your password or activate your account.");
                return CurrentUmbracoPage().WithModel(TempData, model);
            }

            return RedirectToUmbracoPage((int)WellKnownPage.Register, new NameValueCollection {{"email", model.Email}});
        }

        [ChildActionOnly]
        public ActionResult RegisterForm(string email, string token, string pid)
        {
            if (string.IsNullOrEmpty(email) && (!string.IsNullOrWhiteSpace(pid) && !Identity.IsValid(pid)))
                ModelState.AddModelError("Token", "The token is not valid. Please request a new reset token.");

            return View(_personRegistrationService.BuildRegisterForm(email, token, pid));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult FormSubmit(RegisterPersonModel model)
        {
            if (!ModelState.IsValid)
                return CurrentUmbracoPage().WithModel(TempData, model);

            var result = _personRegistrationService.RegisterPerson(model);

            if (result == null)
            {
                ModelState.AddModelError("email", "We have a record of this email address. Please select to reset your password.");
                return CurrentUmbracoPage().WithModel(TempData, model);
            }

            return RedirectToUmbracoPage((int)WellKnownPage.AccountPublishedPage);
        }
      
    }
}
