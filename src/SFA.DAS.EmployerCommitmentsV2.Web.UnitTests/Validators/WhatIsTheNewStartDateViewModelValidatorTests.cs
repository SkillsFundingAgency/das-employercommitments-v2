using FluentValidation.TestHelper;
using NUnit.Framework;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using SFA.DAS.EmployerCommitmentsV2.Web.Validators;
using System;
using System.Linq.Expressions;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Validators
{
    public class WhatIsTheNewStartDateViewModelValidatorTests
    {
        private readonly DateTime _stopDate = new DateTime(2020, 12, 10);

        private const string NoDateEnteredError = "Enter the start date with the new training provider";
        private const string YearNotEnteredError = "The start date must include a year";
        private const string MonthNotEnteredError = "The start date must include a month";
        private const string NotARealDateError = "The start date must be a real date";
        private readonly string NewStartDateBeforeStopDateError;
        private string NewStartDateAfterNewEndDateError;

        public WhatIsTheNewStartDateViewModelValidatorTests()
        {
            NewStartDateBeforeStopDateError = $"The start date must be on or after {_stopDate.ToString("MMMM yyyy")}";
        }

        [Test]
        public void WhenNeitherMonthOrYearAreEntered_ThenValidationFails()
        {
            var model = new WhatIsTheNewStartDateViewModel { NewStartMonth = null, NewStartYear = null, StopDate = _stopDate };

            AssertValidationResult(x => x.NewStartDate, model, false, NoDateEnteredError);
        }

        [Test]
        public void WhenMonthIsEnteredButYearIsNot_ThenValidationFails()
        {
            var model = new WhatIsTheNewStartDateViewModel { NewStartMonth = 6, NewStartYear = null, StopDate = _stopDate };

            AssertValidationResult(x => x.NewStartDate, model, false, YearNotEnteredError);
        }

        [Test]
        public void WhenYearIsEnteredButMonthIsNot_ThenValidationFails()
        {
            var model = new WhatIsTheNewStartDateViewModel { NewStartMonth = null, NewStartYear = 2021, StopDate = _stopDate };

            AssertValidationResult(x => x.NewStartDate, model, false, MonthNotEnteredError);
        }

        [TestCase(13, 2020)]
        [TestCase(0, 2020)]
        [TestCase(-1, 2020)]
        [TestCase(6, -2020)]
        public void WhenInvalidMonthOrYearIsEntered_ThenValidationFails(int month, int year)
        {
            var model = new WhatIsTheNewStartDateViewModel { NewStartMonth = month, NewStartYear = year, StopDate = _stopDate };

            AssertValidationResult(x => x.NewStartDate, model, false, NotARealDateError);
        }

        [TestCase(12, 2020)]
        [TestCase(1, 2021)]
        public void WhenNewStartDateIsValidDateThatIsOnOrAfterStopDate_ThenValidationPasses(int? month, int? year)
        {
            var model = new WhatIsTheNewStartDateViewModel { NewStartMonth = month, NewStartYear = year, StopDate = _stopDate };

            AssertValidationResult(x => x.NewStartDate, model, true);
        }

        [TestCase(11, 2020)]
        public void WhenNewStartDateIsBeforeStopDate_ThenValidationFails(int? month, int? year)
        {
            var model = new WhatIsTheNewStartDateViewModel { NewStartMonth = month, NewStartYear = year, StopDate = _stopDate };

            AssertValidationResult(x => x.NewStartDate, model, true, NewStartDateBeforeStopDateError);
        }

        [Test]
        public void WhenNewEndDateIsNotNull_AndStartDateIsAfterEndDate_ThenInvalid()
        {
            var model = new WhatIsTheNewStartDateViewModel
            {
                NewStartMonth = _stopDate.AddMonths(2).Month,
                NewStartYear = _stopDate.AddMonths(2).Year,
                NewEndDate = _stopDate.AddMonths(1).Date
            };

            NewStartDateAfterNewEndDateError = $"The start date must be before {model.NewEndDate.Value:MMMM yyyy}";

            AssertValidationResult(x => x.NewStartDate, model, false, NewStartDateAfterNewEndDateError);
        }

        [Test]
        public void WhenNewEndDateIsNotNull_AndStartDateIsBeforeEndDate_ThenValalid()
        {
            var model = new WhatIsTheNewStartDateViewModel
            {
                NewStartMonth = _stopDate.AddMonths(1).Month,
                NewStartYear = _stopDate.AddMonths(1).Year,
                NewEndDate = _stopDate.AddMonths(2).Date
            };

            NewStartDateAfterNewEndDateError = $"The start date must be before {model.NewEndDate.Value:MMMM yyyy}";

            AssertValidationResult(x => x.NewStartDate, model, true);
        }

        private void AssertValidationResult<T>(Expression<Func<WhatIsTheNewStartDateViewModel, T>> property, WhatIsTheNewStartDateViewModel instance, bool expectedValid, string expectedErrorMessage = null)
        {
            var validator = new WhatIsTheNewStartDateViewModelValidator();

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
