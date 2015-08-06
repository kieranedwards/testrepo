using System.Web.Mvc;
using Exerp.Api.DataTransfer;
using Exerp.Api.Interfaces.Services;
using PureRide.Web.ViewModels.Account;
using Umbraco.Web;
using Umbraco.Web.Mvc;

namespace PureRide.Web.Controllers.Account
{ 
        public class ResetPasswordController  : SurfaceController 
{
            private readonly IPersonService _personService;

            public ResetPasswordController(IPersonService personService) : this(UmbracoContext.Current)
            {
                _personService = personService;
            }

            public ResetPasswordController(UmbracoContext umbracoContext)
            : base(umbracoContext)
        {
        }

        public ActionResult Index()
        {
            return View();
        }

        [ChildActionOnly]
        public ActionResult ResetPasswordForm(string token, string pid)
        {
            if(string.IsNullOrEmpty(token) || string.IsNullOrWhiteSpace(pid)|| !Identity.IsValid(pid))
                ModelState.AddModelError("Token", "The token is not valid. Please request a new reset token.");

            return View(new ResetPasswordModel(){PID=pid,Token=token});
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult FormSubmit(ResetPasswordModel model)
        {
            if (!_personService.UpdatePersonPasswordWithToken(model.NewPassword, model.Token, new Identity(model.PID)))
            {
                ModelState.AddModelError("Token","We were unable to reset using these details please request a new token.");
                return CurrentUmbracoPage();
            }

            return Redirect("/account/login/?action=Reset");
        }
      
    }
}
