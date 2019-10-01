using System;
using System.Linq.Expressions;
using NUnit.Framework;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;
using SFA.DAS.EmployerCommitmentsV2.Web.Validators;
using FluentValidation.TestHelper;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Validators
{
    [TestFixture]
    public class DetailsViewModelValidatorTests
    {
        [TestCase(null, false)]
        [TestCase(CohortDetailsOptions.Send, true)]
        [TestCase(CohortDetailsOptions.Approve, true)]
        public void Validate_Selection_ShouldBeValidated(CohortDetailsOptions? selection, bool expectedValid)
        {
            var model = new DetailsViewModel { Selection = selection };
            AssertValidationResult(request => request.Selection, model, expectedValid);
        }

        private void AssertValidationResult<T>(Expression<Func<DetailsViewModel, T>> property, DetailsViewModel instance, bool expectedValid)
        {
            var validator = new DetailsViewModelValidator();

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
