using System;
using FluentAssertions;
using NUnit.Framework;
using PureRide.Web.Validators;

namespace Web.Tests.Validators
{
    public class ComplexPasswordAttributeTests
    {
        [Test]
        public void When_IsValid_NonString_Exception()
        {
            var subject = new ComplexPasswordAttribute();
            Action a = () => subject.IsValid(1234);
            a.ShouldThrow<InvalidOperationException>();
        }

        [Test]
        public void When_IsValid_Null_ReturnFalse()
        {
            var subject = new ComplexPasswordAttribute();
            var result = subject.IsValid(null);
            result.Should().BeFalse();
        }

        [Test]
        public void When_IsValid_ToEmpty_ReturnFalse()
        {
            var subject = new ComplexPasswordAttribute();
            var result = subject.IsValid("");
            result.Should().BeFalse();
        }

        [Test]
        public void When_IsValid_ToShort_ReturnFalse()
        {
            var subject = new ComplexPasswordAttribute();
            var result = subject.IsValid("a");
            result.Should().BeFalse();
        }

        [Test]
        public void When_IsValid_NoNumbers_ReturnFalse()
        {
            var subject = new ComplexPasswordAttribute();
            var result = subject.IsValid("ABCdefghij");
            result.Should().BeFalse();
        }

        [Test]
        public void When_IsValid_NoMixedCase_ReturnFalse()
        {
            var subject = new ComplexPasswordAttribute();
            var result = subject.IsValid("abcdefghij123");
            result.Should().BeFalse();
        }

        [Test]
        public void When_IsValid_Correct_ReturnTrue()
        {
            var subject = new ComplexPasswordAttribute();
            var result = subject.IsValid("ABCdefghij123");
            result.Should().BeTrue();
        }
 
    }
}
