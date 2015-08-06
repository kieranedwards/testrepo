using System.Web.Mvc;
using PureRide.Web.ApplicationServices.Account;
using PureRide.Web.ViewModels.Account;
using Umbraco.Web;
using Umbraco.Web.Mvc;

namespace PureRide.Web.Controllers.Account
{ 
    [Authorize]
    public class UpdatePasswordController : SurfaceController 
{
        private readonly IPasswordManagementService _passwordManagementService;

        public UpdatePasswordController(IPasswordManagementService passwordManagementService)
            : this(UmbracoContext.Current)
        {
            _passwordManagementService = passwordManagementService;
        }

        public UpdatePasswordController(UmbracoContext umbracoContext)
            : base(umbracoContext)
        {
        }

        public ActionResult Index()
        {
            return View();
        }

        [ChildActionOnly]
        public ActionResult UpdatePasswordForm()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult FormSubmit(UpdatePasswordModel model)
        {
            if (!ModelState.IsValid)
                return CurrentUmbracoPage(); 

            if (!_passwordManagementService.UpdatePassword(model))
            {
                ModelState.AddModelError("CurrentPassword", "You current password is incorrect please re-enter and try again.");
                return CurrentUmbracoPage(); 
            }

            return CurrentUmbracoPage();
        }
      
    }
}
