using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using FluentAssertions;
using NUnit.Framework;
using PureRide.Web.ViewModels.Account;

namespace Web.Tests.ViewModels.Account
{
    [TestFixture]
    public class RegisterModelTests
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

        private UpdateDetailsModel CreateModel(string password, string confirmPassword)
        {
            return new RegisterPersonModel()
            {
                Password = password,
                PasswordConfirm = confirmPassword,
                ReadHealthQuestionnaire = true,
                ReadTerms = true,
                YearOfBirth = 1985,
                MonthOfBirth = 1,
                DayOfBirth = 1,
                FirstName = "Test",
                LastName = "Test",
                ShoeSize = "3",
                Email = "test@test,com",
                MobilePhoneNumber = "07966025822"
            };
        }

    }
}
