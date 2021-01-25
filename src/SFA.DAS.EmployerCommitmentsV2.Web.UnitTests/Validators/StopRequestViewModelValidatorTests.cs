using FluentValidation.TestHelper;
using NUnit.Framework;
using SFA.DAS.CommitmentsV2.Shared.Models;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using SFA.DAS.EmployerCommitmentsV2.Web.Validators;
using SFA.DAS.Testing.AutoFixture;
using System;
using System.Linq.Expressions;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Validators
{
    [TestFixture]
    public class StopRequestViewModelValidatorTests
    {
        private readonly DateTime _stopDate = new DateTime(2020, 12, 10);

        private const string NoDateEnteredError = "Enter the stop date for this apprenticeship";
        private const string YearNotEnteredError = "Enter the stop date for this apprenticeship";
        private const string MonthNotEnteredError = "Enter the stop date for this apprenticeship";
        private const string NotARealDateError = "The stop date must be a real date";
        private readonly string StopDateBeforeStopDateError;
        private string StopDateAfterNewEndDateError;

        public StopRequestViewModelValidatorTests()
        {
            StopDateBeforeStopDateError = $"The start date must be on or after {_stopDate.ToString("MMMM yyyy")}";
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

        [TestCase(12, 2020)]
        [TestCase(1, 2021)]
        public void WhenStopDateIsValidDateThatIsOnOrAfterStopDate_ThenValidationPasses(int? month, int? year)
        {
            var model = new StopRequestViewModel { StopMonth = month, StopYear = year };

            AssertValidationResult(x => x.StopDate, model, true);
        }

        [TestCase(11, 2020)]
        public void WhenStopDateIsBeforeStopDate_ThenValidationFails(int? month, int? year)
        {
            var model = new StopRequestViewModel { StopMonth = month, StopYear = year };

            AssertValidationResult(x => x.StopDate, model, false, StopDateBeforeStopDateError);
        }

        [Test]
        public void WhenNewEndDateIsNotNull_AndStartDateIsAfterEndDate_ThenInvalid()
        {
            var model = new StopRequestViewModel
            {
                StopMonth = _stopDate.AddMonths(2).Month,
                StopYear = _stopDate.AddMonths(2).Year,
                StartDate = _stopDate.AddMonths(1).Date
            };

            StopDateAfterNewEndDateError = $"The start date must be before {model.StartDate:MMMM yyyy}";

            AssertValidationResult(x => x.StopDate, model, false, StopDateAfterNewEndDateError);
        }

        [Test]
        public void WhenNewEndDateIsNotNull_AndStartDateIsBeforeEndDate_ThenValalid()
        {
            var model = new StopRequestViewModel
            {
                StopMonth = _stopDate.AddMonths(1).Month,
                StopYear = _stopDate.AddMonths(1).Year,
                StartDate = _stopDate.AddMonths(2).Date
            };

            StopDateAfterNewEndDateError = $"The start date must be before {model.StartDate:MMMM yyyy}";

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
