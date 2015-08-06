using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using PureRide.Web.Validators;

namespace PureRide.Web.ViewModels.Account
{
    public class UpdatePasswordModel : PasswordEditModel
    {
        [Required]
        [Display(Name = "Current Password")]
        public string CurrentPassword { get; set; }



    }

    public abstract class PasswordEditModel :IValidatableObject
    {
        [Required, ComplexPassword]
        [Display(Name = "New Password")]
        public string NewPassword { get; set; }

        [Required]
        [Display(Name = "Confirm New Password")]
        public string ConfirmNewPassword { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.Compare(NewPassword, ConfirmNewPassword, StringComparison.CurrentCulture) == 0)
                return Enumerable.Empty<ValidationResult>();

            return new List<ValidationResult>{
                new ValidationResult("Your new password must match the confirmed password",new[]{"Password"})
            };
        }
    }

}
