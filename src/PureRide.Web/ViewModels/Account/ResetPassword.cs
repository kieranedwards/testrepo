using System.ComponentModel.DataAnnotations;

namespace PureRide.Web.ViewModels.Account
{
    public class ResetPasswordModel : PasswordEditModel
    { 
        [Required]
        public string PID { get; set; }

        [Required]
        public string Token { get; set; }

    }
}
