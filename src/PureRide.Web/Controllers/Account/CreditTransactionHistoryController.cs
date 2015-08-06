using System.Web.Mvc;
using PureRide.Web.ApplicationServices.Account;
using PureRide.Web.Helpers;
using Umbraco.Web;
using Umbraco.Web.Models;
using Umbraco.Web.Mvc;

namespace PureRide.Web.Controllers.Account
{
    [Authorize]
    public class CreditTransactionHistoryController : RenderMvcController
    {
        private readonly ICreditHistoryViewModelBuilder _creditHistoryViewModelBuilder;

        public CreditTransactionHistoryController(ICreditHistoryViewModelBuilder creditHistoryViewModelBuilder)
            : this(UmbracoContext.Current)
        {
            _creditHistoryViewModelBuilder = creditHistoryViewModelBuilder;
        }

        public CreditTransactionHistoryController(UmbracoContext umbracoContext)
            : base(umbracoContext)
        {
        }

        public override ActionResult Index(RenderModel model)
        {
            var creditHistoryModel = _creditHistoryViewModelBuilder.Build();
            return base.Index(creditHistoryModel.AsRenderModel());
        }

    }
}
