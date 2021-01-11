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
        private DateTime _stopDate = new DateTime(2020, 12, 10);

        // this could be broken down to test each scenario and excepted outcome
        [TestCase(12, 2000, true)]
        [TestCase(13, 2000, false)]
        [TestCase(null, 2000, false)]
        [TestCase(1, null, false)]
        [TestCase(null, null, false)]
        public void ValidateRealDate(int? month, int? year, bool expectedValid)
        {
            var model = new WhatIsTheNewStartDateViewModel { NewStartMonth = month, NewStartYear = year };

            AssertValidationResult(x => x.NewStartDate, model, expectedValid);
        }

        [TestCase(12, 2020, true)]
        public void ValidateNewStartDateIsOnOrAfterStopDate(int? month, int? year, bool expectedValid)
        {

        }

        private void AssertValidationResult<T>(Expression<Func<WhatIsTheNewStartDateViewModel, T>> property, WhatIsTheNewStartDateViewModel instance, bool expectedValid)
        {
            var validator = new WhatIsTheNewStartDateViewModelValidator();

            if (expectedValid)
            {
                validator.ShouldNotHaveValidationErrorFor(property, instance);
            }
            else
            {
                validator.ShouldHaveValidationErrorFor(property, instance);
            }
        }
    }
}
