using System;
using System.ComponentModel.DataAnnotations;
using PureRide.Web.Validators;
using SagePay.Interfaces;

namespace PureRide.Web.ViewModels.Credits
{
    public class BillingAddressModel : IAddress
    {
        [MaxLength(SagePay.Constants.NameMaxLength)]
        [Display(Name = "First Name")]
        public string FirstNames { get; set; }

        [MaxLength(SagePay.Constants.NameMaxLength)]
        [Display(Name = "Last Name")]
        public string Surname { get; set; }

        [Required(ErrorMessage = "The first address line is required"), MaxLength(SagePay.Constants.AddressLineMaxLength)]
        [Display(Name="Address Line 1")]
        public string Address1 { get; set; }

        [MaxLength(SagePay.Constants.AddressLineMaxLength)]
        [Display(Name = "Address Line 2")]
        public string Address2 { get; set; }

        [Required, MaxLength(SagePay.Constants.CityMaxLength)]
        public string City { get; set; }

        public string County { get; set; }

        [Required]
        public string Country { get; set; }

        [Required, UkPostCode, MaxLength(SagePay.Constants.PostCodeMaxLength)]
        public string PostCode { get; set; }
    }
}