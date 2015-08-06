using System.Web.Mvc;
using PureRide.Web.ApplicationServices.Account;
using PureRide.Web.Helpers;
using PureRide.Web.ViewModels.Account;
using Umbraco.Web;
using Umbraco.Web.Mvc;

namespace PureRide.Web.Controllers.Account
{ 
    [Authorize]
    public class UpdateDetailsController  : SurfaceController 
{
        private readonly IUpdateDetailsModelBuilder _updateDetailsModelBuilder;
        
        public UpdateDetailsController(IUpdateDetailsModelBuilder updateDetailsModelBuilder)
            : this(UmbracoContext.Current)
        {
            _updateDetailsModelBuilder = updateDetailsModelBuilder;
        }

        public UpdateDetailsController(UmbracoContext umbracoContext)
            : base(umbracoContext)
        {
        }

       
        public ActionResult Index()
        {
            return View();
        }

        [ChildActionOnly]
        public ActionResult UpdateDetailsForm()
        {
            return View(_updateDetailsModelBuilder.BuildModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult FormSubmit(UpdateDetailsModel model)
        {
            if(!ModelState.IsValid)
                return CurrentUmbracoPage().WithModel(TempData,model);

            _updateDetailsModelBuilder.UpdatePerson(model);

            return CurrentUmbracoPage();
        }
    }
}
