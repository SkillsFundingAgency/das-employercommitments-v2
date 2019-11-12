using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using FluentValidation.TestHelper;
using NUnit.Framework;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.DraftApprenticeship;
using SFA.DAS.EmployerCommitmentsV2.Web.Validators;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Validators
{
    [TestFixture]
    public class DeleteDraftApprenticeshipViewModelValidatorTests
    {

        [TestCase(true,true)]
        [TestCase(false, true)]
        [TestCase(null, false)]
        public void ThenConfirmDeleteIsValidated(bool? confirmDeleteValue, bool expectedValid)
        {
            var model = new DeleteDraftApprenticeshipViewModel() {ConfirmDelete = confirmDeleteValue};
            AssertValidationResult(x => x.ConfirmDelete, model, expectedValid);
        }

        private void AssertValidationResult<T>(Expression<Func<DeleteDraftApprenticeshipViewModel, T>> property, DeleteDraftApprenticeshipViewModel instance, bool expectedValid)
        {
            var validator = new DeleteDraftApprenticeshipViewModelValidator();

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
