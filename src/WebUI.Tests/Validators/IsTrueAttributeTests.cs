using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using PureRide.Web.Validators;

namespace Web.Tests.Validators
{
    [TestFixture]
    public class IsTrueAttributeTests
    {

        [Test]
        public void When_IsValid_True_ReturnTrue()
        {
            var subject = new IsTrueAttribute();
            var result = subject.IsValid(true);
            result.Should().BeTrue();
        }
        
        [Test]
        public void When_IsValid_False_ReturnFalse()
        {
            var subject = new IsTrueAttribute();
            var result = subject.IsValid(false);
            result.Should().BeFalse();
        }

        [Test]
        public void When_IsValid_abc_Exception()
        {
            var subject = new IsTrueAttribute();
            Action a = () => subject.IsValid("abc"); 
            a.ShouldThrow<InvalidOperationException>();
        }

    }
}
