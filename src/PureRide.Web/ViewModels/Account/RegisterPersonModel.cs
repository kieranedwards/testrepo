using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using PureRide.Web.Validators;

namespace PureRide.Web.ViewModels.Account
{
    public class RegisterPersonModel : UpdateDetailsModel, IValidatableObject
    {

        [Required, ComplexPassword] 
        public string Password { get; set; }

        [Required]//required to be match password
        [Display(Name = "Confirm Password")]
        public string PasswordConfirm { get; set; }

        [IsTrue(ErrorMessage = "Please ensure you have read and accepted the Health Agreement Declaration")]
        public bool ReadHealthQuestionnaire { get; set; }

        [IsTrue(ErrorMessage = "Please ensure you have read and accepted the Terms")]
        public bool ReadTerms { get; set; }

        public bool EmailOptIn { get; set; }

        public bool SmsOptIn { get; set; }

        public string Token { get; set; }

        public string Pid { get; set; }
        
        public new IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var result = new List<ValidationResult>();

            ValidatePassword(result);
            result.AddRange(base.Validate(validationContext));

            return result;
        }

        private void ValidatePassword(ICollection<ValidationResult> result)
        {
            if (Password != PasswordConfirm)
                result.Add(new ValidationResult("The password and confirmed password must match.",
                    new[] { "Password", "Confirm Password" }));
        }

    }
}
