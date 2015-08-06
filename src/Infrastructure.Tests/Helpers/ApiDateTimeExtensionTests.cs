using System;
using Exerp.Api.Helpers;
using FluentAssertions;
using NUnit.Framework;

namespace Exerp.Api.Tests.Helpers
{
    [TestFixture]
    public class ApiDateTimeExtensionTests
    {
        [Test]
        public void When_Converting_Time_To_Api_Format_Return_String()
        {
            var subject = new DateTime(2001, 1, 1,13,45,00);
            var result =subject.ToApiTime();
            result.Should().BeEquivalentTo("13:45");
        }

        [Test]
        public void When_Converting_Date_To_Api_Format_Return_String()
        {
            var subject = new DateTime(2001, 1, 1,13,45,00);
            var result =subject.ToApiDate();
            result.Should().BeEquivalentTo("2001-01-01");
        }
  

        [Test]
        public void When_Converting_Date_From_Api_Format_Return_Date()
        {
            var subject = "2001-01-21";
            var result = subject.FromApiDateString();
            result.Should().Be(new DateTime(2001, 1, 21, 0, 0, 0));
        }

        [Test]
        public void When_Converting_Date_From_Api_Format_EmptyString_Return_MinDate()
        {
            var subject = "";
            var result = subject.FromApiDateString();
            result.Should().Be(DateTime.MinValue);
        }


        [Test]
        public void When_Converting_DateTime_From_Api_Format_Return_Date()
        {
            var subjectDate = "2001-01-01";
            var subjectTime = "13:45";
            var result = subjectDate.FromApiDateTimeString(subjectTime);
            result.Should().Be(new DateTime(2001, 1, 1, 13, 45, 0));
        }

        public void When_Converting_DateTime_From_Api_Format_With_EmptyString_Return_MinDate()
        {
            var subjectDate = "2001-01-01";
            var subjectTime = "";
            var result = subjectDate.FromApiDateTimeString(subjectTime);
            result.Should().Be(DateTime.MinValue);
        }

    }
}
