using System.Collections.Generic;
using Exerp.Api.DataTransfer;

namespace PureRide.Web.ViewModels.Account
{
    public class AccountDashboardCoreModel
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string ShoeSize { get; set; }
        public string Birthday { get; set; }
        public int FriendsCount { get; set; }
    }

    public class AccountDashboardCreditsModel
    {
        public string Region { get; set; }
        public int Credits { get; set; }
    }

    public class AccountDashboardBookingsModel
    {
        public IEnumerable<KeyValuePair<int, IEnumerable<Participation>>> Bookings { get; set; }
    }
}
