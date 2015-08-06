using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using FluentAssertions;
using NUnit.Framework;
using PureRide.Web.ViewModels.Account;

namespace Web.Tests.ViewModels.Account
{
    [TestFixture]
    public class UpdateDetailsModelTests
    {

        [Test]
        public void When_Validate_ValidDateOfBirth_NoErrors()
        {
            var subject = CreateModel(1,1,1985);

            var result = ValidateModel(subject);

            result.Should().BeEmpty();
        }

        [Test]
        public void When_Validate_Age18DateOfBirth_NoErrors()
        {
            var date = DateTime.Today.AddYears(-18);
            var subject = CreateModel(date.Day, date.Month, date.Year);  

            var result = ValidateModel(subject);

            result.Should().BeEmpty();
        }

        [Test]
        public void When_Validate_Age17DateOfBirth_Error()
        {
            var date = DateTime.Today.AddYears(-18).AddDays(1);
            var subject = CreateModel(date.Day, date.Month, date.Year);
            var result = ValidateModel(subject);

            result.Should().OnlyContain(a => a.ErrorMessage.Contains("18"));
        }

        [Test]
        public void When_Validate_InvalidDate_Error()
        {
            var subject = CreateModel(31, 2, 1901);

            var result = ValidateModel(subject);

            result.Should().OnlyContain(a => a.ErrorMessage.Contains("format"));
        }


        [Test]
        public void When_ShoeSizeOptions_NoShowSize_NoSelectedFlag()
        {
            var subject = CreateModel(31, 2, 1901);
            var result = subject.ShoeSizeOptions();
            result.Should().NotContain(a => a.Selected);
        }

        [Test]
        public void When_ShoeSizeOptions_ShowSizeSelected_SelectedFlagSet()
        {
            var subject = CreateModel(31, 2, 1901);
            subject.ShoeSize = "34 / 2.5";
            var result = subject.ShoeSizeOptions();
            result.Should().ContainSingle(a => a.Selected);
        }

        [Test]
        public void When_ShoeSizeOptions_EmptyModel_NotEmptyList()
        {
            var subject = new UpdateDetailsModel();
            var result = subject.ShoeSizeOptions();
            result.Should().NotBeEmpty();
        }

        private IList<ValidationResult> ValidateModel(object model)
        {
            var validationResults = new List<ValidationResult>();
            var ctx = new ValidationContext(model, null, null);
            Validator.TryValidateObject(model, ctx, validationResults, true);
            return validationResults;
        }

        [Test]
        public void When_Get_DOB_Reads_From_Model_Return_Date()
        {
            var subject = CreateModel(31, 1, 1901);
            var result = subject.GetDateOfBirth();
            result.ShouldBeEquivalentTo(new DateTime(1901, 1, 31));
        }

        [Test]
        public void When_Get_DOB_Reads_From_Model_UnderAge_Error()
        {
            var subject = CreateModel(31, 1, 2001);
            Action action = () => subject.GetDateOfBirth();
            action.ShouldThrow<ArgumentException>();
        }

        private UpdateDetailsModel CreateModel(int day, int month, int year)
        {
            return new UpdateDetailsModel
            {
                YearOfBirth = year,
                MonthOfBirth = month,
                DayOfBirth = day,
                FirstName = "Test",
                LastName = "Test",
                Email = "test@test,com",
                ShoeSize = "3",
                MobilePhoneNumber = "07966025822"
            };
        }
        
    }
}
