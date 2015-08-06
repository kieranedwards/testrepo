using System.Web.Mvc;
using Exerp.Api.Interfaces.Services;
using PureRide.Web.ApplicationServices.Account;
using PureRide.Web.Helpers;
using PureRide.Web.ViewModels.Account;
using Umbraco.Web;
using Umbraco.Web.Mvc;

namespace PureRide.Web.Controllers.Account
{ 
    public class ForgottenPasswordController  : SurfaceController 
{
        private readonly IPasswordManagementService _personService;

        public ForgottenPasswordController(IPasswordManagementService personService)
            : this(UmbracoContext.Current)
        {
            _personService = personService;
        }

        public ForgottenPasswordController(UmbracoContext umbracoContext)
            : base(umbracoContext)
        {
        }

       
        public ActionResult Index()
        {
            return View();
        }

        [ChildActionOnly]
        public ActionResult ForgottenPasswordForm()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult FormSubmit(AccountEmailModel model)
        {
            if (!ModelState.IsValid)
                return CurrentUmbracoPage().WithModel(TempData, model);

            if (_personService.SendPasswordResetToken(model.Email))
                return Redirect("/account/login?action=token");

            ModelState.AddModelError("Email", "We were unable to send a password reset email to this email address as it not registered.");
            return CurrentUmbracoPage();
        }
        
    }
}
