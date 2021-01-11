using NUnit.Framework;
using SFA.DAS.CommitmentsV2.Shared.Models;
using SFA.DAS.EmployerCommitmentsV2.Web.Extensions;
using System;

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

        //Expand to separate tests to explain cases better
        [TestCase(2019, 1, 1, true)]
        [TestCase(2020, 1, 1, true)] 
        [TestCase(2020, 3, 1, true)]
        [TestCase(2020, 3, 31, true )]
        [TestCase(2020, 5, 1, false)]
        public void IsGreaterThanOrEqualToDateTimeMonthYear(int year, int month, int day, bool expectedResult)
        {
            var date = new DateTime(year, month, day);

            var actualResult = _monthYear.IsGreaterThanOrEqualToDateTimeMonthYear(date);

            Assert.AreEqual(expectedResult, actualResult);
        }
    }
}
