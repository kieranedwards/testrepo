using System.Web.Http;
using System.Web.Mvc;
using PureRide.Web.ApplicationServices.Account;
using PureRide.Web.ViewModels.Account;
using Umbraco.Web.Mvc;

namespace PureRide.Web.Controllers.Account
{
    public class AccountApiController : ApiController
    {
        private readonly IPersonRegistrationService _personRegistrationService;
        private readonly IAccountDashboardViewModelBuilder _accountDashboardViewModelBuilder;

        public AccountApiController(IPersonRegistrationService personRegistrationService,IAccountDashboardViewModelBuilder accountDashboardViewModelBuilder)
        {
            _personRegistrationService = personRegistrationService;
            _accountDashboardViewModelBuilder = accountDashboardViewModelBuilder;
        }

        [System.Web.Http.HttpGet]
        public ActionResult GetBookings(bool includePast)
        {
            return new JsonNetResult() { Data = _accountDashboardViewModelBuilder.BuildBookingsModel(includePast) };
        }

        [System.Web.Http.HttpGet]
        public ActionResult GetCreditBalance()
        {
            return new JsonNetResult() { Data = _accountDashboardViewModelBuilder.BuildCreditStatusModel() };
        }

        [System.Web.Http.HttpPost]
        public ActionResult AddFriend(RegisterFriendModel model)
        {
            return new JsonNetResult() { Data = _personRegistrationService.RegisterFriend(model).ToString()};
        }
    }
}
