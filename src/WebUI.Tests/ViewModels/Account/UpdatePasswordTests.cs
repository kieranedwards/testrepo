using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using PureRide.Web.ViewModels.Account;

namespace Web.Tests.ViewModels.Account
{
    [TestFixture]
    public class UpdatePasswordTests
    {

        [Test]
        public void When_Validate_MatchingPasswords_NoError()
        {
            var subject = CreateModel("Abc123456", "Abc123456");

            var result = ValidateModel(subject);

            result.Should().BeEmpty();
        }

        [Test]
        public void When_Validate_NonMatchingPasswords_Error()
        {
            var subject = CreateModel("Abc123456", "Ab123456");

            var result = ValidateModel(subject);

            result.Should().OnlyContain(a => a.ErrorMessage.Contains("match"));
        }

        private IList<ValidationResult> ValidateModel(object model)
        {
            var validationResults = new List<ValidationResult>();
            var ctx = new ValidationContext(model, null, null);
            Validator.TryValidateObject(model, ctx, validationResults, true);
            return validationResults;
        }

        private PasswordEditModel CreateModel(string password, string confirmPassword)
        {
            return new UpdatePasswordModel()
            {
                CurrentPassword = "Test",
                NewPassword = password,
                ConfirmNewPassword = confirmPassword
            };
        }


    }
}
