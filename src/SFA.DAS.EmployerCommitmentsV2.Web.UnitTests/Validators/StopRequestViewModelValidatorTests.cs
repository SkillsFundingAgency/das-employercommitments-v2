using FluentValidation.TestHelper;
using NUnit.Framework;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using SFA.DAS.EmployerCommitmentsV2.Web.Validators;
using System;
using System.Linq.Expressions;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Validators
{
    [TestFixture]
    public class StopRequestViewModelValidatorTests
    {
        private readonly DateTime _startDate = new DateTime(2020, 12, 10);

        private const string NoDateEnteredError = "Enter the stop date for this apprenticeship";
        private const string YearNotEnteredError = "Enter the stop date for this apprenticeship";
        private const string MonthNotEnteredError = "Enter the stop date for this apprenticeship";
        private const string NotARealDateError = "The stop date must be a real date";
        private const string StopDateIsInFutureError = "The stop date cannot be in the future";
        private readonly string StopDateBeforeStartDateError = "The stop month cannot be before the apprenticeship started";

        public StopRequestViewModelValidatorTests()
        {
        }

        [Test]
        public void WhenNeitherMonthOrYearAreEntered_ThenValidationFails()
        {
            var model = new StopRequestViewModel { StopMonth = null, StopYear = null };

            AssertValidationResult(x => x.StopDate, model, false, NoDateEnteredError);
        }

        [Test]
        public void WhenMonthIsEnteredButYearIsNot_ThenValidationFails()
        {
            var model = new StopRequestViewModel { StopMonth = 6, StopYear = null };

            AssertValidationResult(x => x.StopDate, model, false, YearNotEnteredError);
        }

        [Test]
        public void WhenYearIsEnteredButMonthIsNot_ThenValidationFails()
        {
            var model = new StopRequestViewModel { StopMonth = null, StopYear = 2021 };

            AssertValidationResult(x => x.StopDate, model, false, MonthNotEnteredError);
        }

        [TestCase(13, 2020)]
        [TestCase(0, 2020)]
        [TestCase(-1, 2020)]
        [TestCase(6, -2020)]
        public void WhenInvalidMonthOrYearIsEntered_ThenValidationFails(int month, int year)
        {
            var model = new StopRequestViewModel { StopMonth = month, StopYear = year };

            AssertValidationResult(x => x.StopDate, model, false, NotARealDateError);
        }

        [Test]
        public void WhenStopDateIsInFuture_ThenValidationFails()
        {
            var futureStopDate = DateTime.Now.AddMonths(1);
            var model = new StopRequestViewModel { StopMonth = futureStopDate.Month, StopYear = futureStopDate.Year };

            AssertValidationResult(x => x.StopDate, model, false, StopDateIsInFutureError);
        }

        [TestCase(11, 2020)]
        [TestCase(10, 2020)]
        public void WhenStopDateIsBeforeStartDate_ThenValidationFails(int? month, int? year)
        {
            var model = new StopRequestViewModel { StopMonth = month, StopYear = year, StartDate = _startDate };

            AssertValidationResult(x => x.StopDate, model, false, StopDateBeforeStartDateError);
        }

        [TestCase(12, 2020)]
        public void WhenStopDateIsEqualToStartDate_ThenValidates(int? month, int? year)
        {
            var model = new StopRequestViewModel { StopMonth = month, StopYear = year, StartDate = _startDate };

            AssertValidationResult(x => x.StopDate, model, true);
        }

        private void AssertValidationResult<T>(Expression<Func<StopRequestViewModel, T>> property, StopRequestViewModel instance, bool expectedValid, string expectedErrorMessage = null)
        {
            var validator = new StopRequestViewModelValidator();

            if (expectedValid)
            {
                validator.ShouldNotHaveValidationErrorFor(property, instance);
            }
            else
            {
                validator.ShouldHaveValidationErrorFor(property, instance).WithErrorMessage(expectedErrorMessage);
            }
        }
    }
}
