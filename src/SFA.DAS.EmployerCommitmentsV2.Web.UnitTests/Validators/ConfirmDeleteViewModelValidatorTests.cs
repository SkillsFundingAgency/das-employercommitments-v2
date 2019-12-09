using System;
using System.Linq.Expressions;
using NUnit.Framework;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;
using SFA.DAS.EmployerCommitmentsV2.Web.Validators;
using FluentValidation.TestHelper;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Validators
{
    [TestFixture]
    public class ConfirmDeleteViewModelValidatorTests
    {
        [TestCase(null, false)]
        [TestCase(true, true)]
        [TestCase(false, true)]
        public void Validate_ConfirmDeletion_ShouldBeValidated(bool? confirmDeletion, bool expectedValid)
        {
            var model = new ConfirmDeleteViewModel() { ConfirmDeletion = confirmDeletion };
            AssertValidationResult(request => request.ConfirmDeletion, model, expectedValid);
        }

        private void AssertValidationResult<T>(Expression<Func<ConfirmDeleteViewModel, T>> property, ConfirmDeleteViewModel instance, bool expectedValid)
        {
            var validator = new ConfirmDeleteViewModelValidator();

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
