using System;
using FluentAssertions;
using NUnit.Framework;
using PureRide.Web.Providers;

namespace Web.Tests.Providers
{
    [TestFixture]
    public class DisplayDateProviderTests
    {

        [TestCase(0, "Mon 01/01")]
        [TestCase(1, "Tue 02/01")]
        [TestCase(2, "Wed 03/01")]
        [TestCase(3, "Thu 04/01")]
        [TestCase(4, "Fri 05/01")]
        [TestCase(5, "Sat 06/01")]
        [TestCase(6, "Sun 07/01")]
        [TestCase(7, "Mon 08/01")]
        [TestCase(8, "Tue 09/01")]
        [TestCase(9, "Wed 10/01")]
        [TestCase(10, "Thu 11/01")]
        [TestCase(11, "Fri 12/01")]
        [TestCase(12, "Sat 13/01")]
        [TestCase(13, "Sun 14/01")]
        [TestCase(14, "Mon 15/01")]
        [TestCase(15, "Tue 16/01")]
        [TestCase(16, "Wed 17/01")]
        [TestCase(17, "Thu 18/01")]
        [TestCase(18, "Fri 19/01")]
        [TestCase(19, "Sat 20/01")]
        [TestCase(20, "Sun 21/01")]
        [TestCase(21, "Mon 22/01")]
        [TestCase(22, "Tue 23/01")]
        [TestCase(23, "Wed 24/01")]
        [TestCase(24, "Thu 25/01")]
        [TestCase(25, "Fri 26/01")]
        [TestCase(26, "Sat 27/01")]
        [TestCase(27, "Sun 28/01")]
        [TestCase(28, "Mon 29/01")]
        [TestCase(29, "Tue 30/01")]
        [TestCase(30, "Wed 31/01")]
        public void When_Format_WithDate_ThenPrintFormated(int dateOffSet,string expectedResult)
        {
            var date = new DateTime(2001, 01, 01).AddDays(dateOffSet);
            string.Format(new DisplayDateProvider(), "{0}", date).Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public void When_Format_WithToday_ThenPrintToday()
        {
            var date = DateTime.Today;
            string.Format(new DisplayDateProvider(), "{0}", date).Should().BeEquivalentTo("Today");
        }

        [Test]
        public void When_Format_WithTomorrow_ThenPrint()
        {
            var date = DateTime.Today.AddDays(1);
            string.Format(new DisplayDateProvider(), "{0}", date).Should().BeEquivalentTo("Tomorrow");
        }

        [Test]
        public void When_Format_Is_Not_A_Date_Error()
        {
            Action action = () => string.Format(new DisplayDateProvider(), "{0}", "1/1/1");
            action.ShouldThrow<NotSupportedException>();
        }


    }
}
