using System;
using Exerp.Api.DataTransfer;
using FluentAssertions;
using NUnit.Framework;

namespace Exerp.Api.Tests.DataTransfer
{
    [TestFixture]
    public class IdentityTests
    {
        private const int CentreId = 100;
        private const int PersonId = 200;

        [Test]
        public void When_Constructor_WithParameters()
        {
            var result = new Identity(CentreId,PersonId);
            result.CentreId.Should().Be(CentreId);
            result.EntityId.Should().Be(PersonId);
        }

        [Test]
        public void When_Constructor_WithString()
        {
            var result = new Identity(string.Concat(CentreId,":",PersonId));
            result.CentreId.Should().Be(CentreId);
            result.EntityId.Should().Be(PersonId);
        }

        [Test]
        public void When_Constructor_WithNullString()
        {
            // ReSharper disable once ObjectCreationAsStatement
            Action a = () => new Identity(string.Empty);
            a.ShouldThrow<ArgumentException>();
        }

        [Test]
        public void When_Equals_WithNullString()
        {
            var a = new Identity(CentreId, PersonId);
            var b = new Identity(CentreId, PersonId);

            var result = a.Equals(b);
                
            result.Should().BeTrue();
        }

        [Test]
        public void When_Equals_WithDiffrentCentreId_ReturnFalse()
        {
            var a = new Identity(CentreId, PersonId);
            var b = new Identity(0, PersonId);
            a.Equals(b).Should().BeFalse();
        }

        [Test]
        public void When_Equals_WithDiffrentPersonId_ReturnFalse()
        {
            var a = new Identity(CentreId, PersonId);
            var b = new Identity(CentreId, 0);
            a.Equals(b).Should().BeFalse();
        }

        [Test]
        public void When_Validate_WithInvalidPersonId_ReturnFalse()
        {
            var result = Identity.IsValid(string.Concat("xxx:", PersonId));
            result.Should().BeFalse();
        }

        [Test]
        public void When_Validate_WithValidPersonId_ReturnTrue()
        {
            var result = Identity.IsValid(string.Concat(CentreId,":",PersonId));
            result.Should().BeTrue();
        }
    }
}
