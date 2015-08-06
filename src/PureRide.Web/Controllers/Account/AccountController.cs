using System.Web.Mvc;
using PureRide.Web.ApplicationServices.Account;
using PureRide.Web.Helpers;
using Umbraco.Web;
using Umbraco.Web.Models;
using Umbraco.Web.Mvc;

namespace PureRide.Web.Controllers.Account
{ 
    [Authorize]
    public class AccountController  : RenderMvcController 
{
        private readonly IAccountDashboardViewModelBuilder _accountDashboardViewModelBuilder;

        public AccountController(IAccountDashboardViewModelBuilder accountDashboardViewModelBuilder)
            : this(UmbracoContext.Current)
        {
            _accountDashboardViewModelBuilder = accountDashboardViewModelBuilder;
        }

        public AccountController(UmbracoContext umbracoContext)
            : base(umbracoContext)
        {
        }

        public override ActionResult Index(RenderModel model)
        {
            var accountModel = _accountDashboardViewModelBuilder.Build();
            return base.Index(accountModel.AsRenderModel());
        }
         
      
    }
 
}
