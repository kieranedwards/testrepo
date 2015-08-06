using FluentAssertions;
using NUnit.Framework;
using PureRide.Web.Validators;

namespace Web.Tests.Validators
{
    //useful test cases - http://www.doogal.co.uk/PostcodeGenerator.php

    [TestFixture]
    public class UkPostCodeValidationAttributeTests
    {

        [TestCase("WA5 1QS")]
        [TestCase("WA51QS")]
        [TestCase("wa5 1qs")]
        [TestCase("wa51qs")]
        public void When_IsValid_UsingData_ReturnTrue(string postCode)
        {
            var subject = new UkPostCodeAttribute();
            var result = subject.IsValid(postCode);
            result.Should().BeTrue();
        }

        [TestCase("abc")]
        [TestCase("s0316SA")]
        public void When_IsValid_UsingBadData_ReturnFalse(string postCode)
        {
            var subject = new UkPostCodeAttribute();
            var result = subject.IsValid(postCode);
            result.Should().BeFalse();
        }

      
    }
}
