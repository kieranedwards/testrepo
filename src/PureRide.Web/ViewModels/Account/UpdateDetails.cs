using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;

namespace PureRide.Web.ViewModels.Account
{
    public class UpdateDetailsModel :IValidatableObject
    {
        private const string DobFormat = "{0:00}-{1:00}-{2}";

        [Required]
        public string Email { get; set; }

        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
      
        public bool IsMale { get; set; }

        [Required]
        [Display(Name = "Mobile Phone Number")]
        public string MobilePhoneNumber { get; set; }

        [Required]
        [Display(Name = "Shoe Size")]
        public string ShoeSize { get; set; }

        [Display(Name = "Date of Birth")]
        public int DayOfBirth { get; set; }
        public int MonthOfBirth { get; set; }
        public int YearOfBirth { get; set; }


        public List<SelectListItem> ShoeSizeOptions()
        {
            var result = new List<SelectListItem>();
            
            var sourceData = "34,2.5;35,3;36,4;37,4.5;38,5;39,5.5;40,6;41,7;42,8;43,9;44,10;45,10.5;46,11;47,12;48,12.5;49,13;50,14";

            result.Add(new SelectListItem() { Value = "", Text = ""});
            foreach (var size in sourceData.Split(';'))
            {
                var parts = size.Split(',');
                var shoeValue = String.Format("{0} / {1}", parts[0], parts[1]);
                var shoeText = String.Format("EU {0} / UK {1}", parts[0], parts[1]);
                result.Add(new SelectListItem() { Value = shoeValue, Text = shoeText, Selected = (shoeValue==ShoeSize)});
            }

            return result;
        }


        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return ValidateDateOfBirth();
        }

        private IEnumerable<ValidationResult> ValidateDateOfBirth()
        {
            var result = new List<ValidationResult>();
            string[] dobFields = { "DayOfBirth", "MonthOfBirth", "YearOfBirth" };

            DateTime dateOfBirth;

            var dateString = string.Format(DobFormat, DayOfBirth, MonthOfBirth, YearOfBirth);
            if (!DateTime.TryParseExact(dateString, "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateOfBirth))
            {
                result.Add(new ValidationResult("The date of birth must be valid format.", dobFields));
            }
            else if (dateOfBirth > DateTime.Today.AddYears(-18))
            {
                result.Add(new ValidationResult("You must be over 18 to take part in PureRide.", dobFields));
            }

            return result;
        }
        
        public DateTime GetDateOfBirth()
        {
            if(ValidateDateOfBirth().Any())
                throw new ArgumentException("DOB is not valid");

            var dob = string.Format(DobFormat, DayOfBirth, MonthOfBirth, YearOfBirth);
            return DateTime.ParseExact(dob, "dd-MM-yyyy",CultureInfo.CurrentCulture, DateTimeStyles.None);
        }
    }
}
