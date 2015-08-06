using System.ComponentModel.DataAnnotations;

namespace PureRide.Web.ViewModels.Account
{
    public class AccountLoginModel
    {
        public bool IsRecentLogOut { get; set; }

        [Required, DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}
