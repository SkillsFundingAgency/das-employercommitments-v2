using FluentValidation.TestHelper;
using NUnit.Framework;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using SFA.DAS.EmployerCommitmentsV2.Web.Validators;
using System;
using System.Linq.Expressions;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Validators
{
    public class CancelChangeOfProviderRequestViewModelValidatorTests
    {
        private const string SelectAnswerError = "Select if you want to cancel this request";

        [TestCase(true)]
        [TestCase(false)]
        public void WhenYesOrNoIsSelected_ThenValidationPasses(bool cancelRequest)
        {
            var model = new CancelChangeOfProviderRequestViewModel { CancelRequest = cancelRequest };

            AssertValidationResult(x => x.CancelRequest, model, true);
        }

        [Test]
        public void WhenNoAnswerIsSelected_ThenValidationFails()
        {
            var model = new CancelChangeOfProviderRequestViewModel { CancelRequest = null };

            AssertValidationResult(x => x.CancelRequest, model, false, SelectAnswerError);
        }

        private void AssertValidationResult<T>(Expression<Func<CancelChangeOfProviderRequestViewModel, T>> property, CancelChangeOfProviderRequestViewModel instance, bool expectedValid, string expectedErrorMessage = null)
        {
            var validator = new CancelChangeOfProviderRequestViewModelValidator();

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
