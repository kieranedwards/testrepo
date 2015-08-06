using System.ComponentModel.DataAnnotations;

namespace PureRide.Web.ViewModels.Account
{
    public class AccountEmailModel
    {
        [Required]
        public string Email { get; set; }

    }
}
