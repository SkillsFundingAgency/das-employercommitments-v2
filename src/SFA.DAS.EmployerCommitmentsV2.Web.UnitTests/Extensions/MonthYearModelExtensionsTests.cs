using System;
using System.Globalization;
using NUnit.Framework;
using SFA.DAS.CommitmentsV2.Shared.Models;
using SFA.DAS.EmployerCommitmentsV2.Web.Extensions;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Extensions
{
    [TestFixture]
    public class MonthYearModelExtensionsTests
    {
        private MonthYearModel _monthYear;

        [SetUp]
        public void Arrange()
        {
            _monthYear = new MonthYearModel("") { Month = 3, Year = 2020 };
        }

        [TestCase(2019, 1, 1)]
        [TestCase(2020, 1, 1)]
        public void WhenDateTimeIsBeforeMonthYearModel_IsEqualToOrAfterMonthYearOfDateTime_ReturnsTrue(int year, int month, int day)
        {
            var dateTime = new DateTime(year, month, day);

            var actualResult = _monthYear.IsEqualToOrAfterMonthYearOfDateTime(dateTime);

            Assert.That(actualResult, Is.EqualTo(true));
        }

        [TestCase(2020, 3, 1)]
        [TestCase(2020, 3, 31)]
        [TestCase(2020, 3, 31, 23, 59, 59)]
        public void WhenDateTimeIsAnywhereWithinMonthYearModel_IsEqualToOrAfterMonthYearOfDateTime_ReturnsTrue(int year, int month, int day, int hour = 0, int min = 0, int sec = 0)
        {
            var dateTime = new DateTime(year, month, day, hour, min, sec);

            var actualResult = _monthYear.IsEqualToOrAfterMonthYearOfDateTime(dateTime);

            Assert.That(actualResult, Is.EqualTo(true));
        }

        [TestCase(2020, 4, 1)]
        [TestCase(2020, 4, 30)]
        [TestCase(2021, 3, 31)]
        public void WhenDateTimeIsAfterMonthYearModel_IsEqualToOrAfterMonthYearOfDateTime_ReturnsFalse(int year, int month, int day)
        {
            var dateTime = new DateTime(year, month, day);

            var actualResult = _monthYear.IsEqualToOrAfterMonthYearOfDateTime(dateTime);

            Assert.That(actualResult, Is.EqualTo(false));
        }

        [Test]
        public void WhenDateTimeIsBeforeMonthYearModel_IsBeforeMonthYearOfDateTime_ReturnsTrue()
        {
            var dateTime = new DateTime(2020, 6, 1);

            var actualResult = _monthYear.IsBeforeMonthYearOfDateTime(dateTime);

            Assert.That(actualResult, Is.True);
        }

        [TestCase(2020, 3, 1)]
        [TestCase(2020, 1, 1)]
        public void WhenDateTimeIsEqualToOrAfterMonthYearModel_IsBeforeMonthYearOfDateTime_ReturnsFalse(int year, int month, int day)
        {
            var dateTime = new DateTime(year, month, day);

            var actualResult = _monthYear.IsBeforeMonthYearOfDateTime(dateTime);

            Assert.That(actualResult, Is.False);
        }

        [TestCase(2020, 6, 1)]
        [TestCase(2020, 3, 1)]
        public void WhenDateTimeIsEqualToOrAfterMonthYearModel_IsEqualToOrBeforeMonthYearOfDateTime_ReturnsTrue(int year, int month, int day)
        {
            var dateTime = new DateTime(year, month, day);

            var actualResult = _monthYear.IsEqualToOrBeforeMonthYearOfDateTime(dateTime);

            Assert.That(actualResult, Is.True);
        }

        [Test]
        public void WhenDateTimeIsBeforeMonthYearModel_IsEqualToOrBeforeMonthYearOfDateTime_ReturnsFalse()
        {
            var dateTime = new DateTime(2020, 1, 1);

            var actualResult = _monthYear.IsEqualToOrBeforeMonthYearOfDateTime(dateTime);

            Assert.That(actualResult, Is.False);
        }

        [Test]
        [TestCase("01/12/2021", "01/11/2021", true)]
        [TestCase("01/12/2021", "01/01/2022", false)]
        public void WhenDateTimeIsInFuture_IsNotInFutureMonthYear_ReturnsFalse(string nowDateString, string proposedStopDateString, bool isValid)
        {
            var nowDate = DateTime.ParseExact(nowDateString,
                    "dd/MM/yyyy",
                    CultureInfo.CurrentCulture);

            var proposedStopDate = DateTime.ParseExact(proposedStopDateString,
                  "dd/MM/yyyy",
                  CultureInfo.CurrentCulture);

            var monthYear = new MonthYearModel(proposedStopDate.ToString("MMyyyy"));

            var actualResult = monthYear.IsNotInFutureMonthYear(nowDate);

            Assert.That(actualResult, Is.EqualTo(isValid));
        }
    }
}
