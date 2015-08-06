using System.ComponentModel.DataAnnotations;

namespace PureRide.Web.ViewModels.Account
{
    public class RegisterFriendModel
    {
        [Required]
        public string Email { get; set; }

        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Display(Name = "Mobile Phone Number")]
        public string MobilePhoneNumber { get; set; }
    }
}
