using FluentValidation.TestHelper;
using NUnit.Framework;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using SFA.DAS.EmployerCommitmentsV2.Web.Validators;
using System;
using System.Linq.Expressions;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Validators
{
    public class EmployerLedChangeOfProviderRequestValidatorTests
    {
        [TestCase("5143541", true)]
        [TestCase(" ", false)]
        [TestCase("", false)]
        [TestCase(null, false)]
        public void ThenAccountHashedIdIsValidated(string accountHashedId, bool expectedValid)
        {
            var request = new EmployerLedChangeOfProviderRequest() { AccountHashedId = accountHashedId };
            AssertValidationResult(x => x.AccountHashedId, request, expectedValid);
        }
        [TestCase("", false)]
        [TestCase("31", true)]
        [TestCase(" ", false)]
        [TestCase(null, false)]
        public void ThenApprenticeshipHashedIdIsValidated(string apprenticeshipHashedId, bool expectedValid)
        {
            var request = new EmployerLedChangeOfProviderRequest() { ApprenticeshipHashedId = apprenticeshipHashedId };
            AssertValidationResult(x => x.ApprenticeshipHashedId, request, expectedValid);
        }

        [TestCase(default(long), false)]
        [TestCase(-2342, false)]
        [TestCase(234, true)]
        public void ThenProviderIsValidated(long providerId, bool expectedValid)
        {
            var request = new EmployerLedChangeOfProviderRequest() { ProviderId = providerId };
            AssertValidationResult(x => x.ProviderId, request, expectedValid);
        }

        private void AssertValidationResult<T>(Expression<Func<EmployerLedChangeOfProviderRequest, T>> property, EmployerLedChangeOfProviderRequest instance, bool expectedValid)
        {
            var validator = new EmployerLedChangeOfProviderRequestValidator();

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
