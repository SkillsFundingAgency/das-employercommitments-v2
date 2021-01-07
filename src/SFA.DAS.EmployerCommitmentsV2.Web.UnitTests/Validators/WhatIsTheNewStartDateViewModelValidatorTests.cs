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
        [TestCase(12, 2000, true)]
        [TestCase(13, 2000, false)]
        [TestCase(null, 2000, false)]
        [TestCase(1, null, false)]
        [TestCase(null, null, false)]
        public void Validate(int? month, int? year, bool expectedValid)
        {
            var model = new WhatIsTheNewStartDateViewModel { NewStartMonth = month, NewStartYear = year };

            AssertValidationResult(x => x.NewStartDate, model, expectedValid);
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
