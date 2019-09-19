using System;
using System.Linq.Expressions;
using FluentValidation.TestHelper;
using NUnit.Framework;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;
using SFA.DAS.EmployerCommitmentsV2.Web.Validators;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Validators
{
    [TestFixture()]
    public class CreateCohortWithDraftApprenticeshipRequestValidatorTests
    {
        [TestCase(-1, false)]
        [TestCase(0, false)]
        [TestCase(123, true)]
        public void Validate_ProviderId_ShouldBeValidated(long providerId, bool expectToBeValid)
        {
            var model = new ApprenticeRequest { ProviderId = providerId};
            AssertValidationResult(request => request.ProviderId, model, expectToBeValid);
        }

        [TestCase(-1, false)]
        [TestCase(0, false)]
        [TestCase(123, true)]
        public void Validate_StartDate_ShouldBeValidated(long accountLegalEntityId, bool expectedValid)
        {
            var model = new ApprenticeRequest { AccountLegalEntityId = accountLegalEntityId };
            AssertValidationResult(request => request.AccountLegalEntityId, model, expectedValid);
        }

        private void AssertValidationResult<T>(Expression<Func<ApprenticeRequest, T>> property, ApprenticeRequest instance, bool expectedValid)
        {
            var validator = new CreateCohortWithDraftApprenticeshipRequestValidator();

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