using FluentValidation.TestHelper;
using NUnit.Framework;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using SFA.DAS.EmployerCommitmentsV2.Web.Validators;
using System;
using System.Linq.Expressions;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Validators
{
    public class ChangeOfProviderRequestValidatorTests
    {
        [TestCase("5143541", true)]
        [TestCase(" ", false)]
        [TestCase("", false)]
        [TestCase(null, false)]
        public void ThenAccountHashedIdIsValidated(string accountHashedId, bool expectedValid)
        {
            var request = new ChangeOfProviderRequest() { AccountHashedId = accountHashedId };
            AssertValidationResult(x => x.AccountHashedId, request, expectedValid);
        }
        [TestCase("", false)]
        [TestCase("31", true)]
        [TestCase(" ", false)]
        [TestCase(null, false)]
        public void ThenApprenticeshipHashedIdIsValidated(string apprenticeshipHashedId, bool expectedValid)
        {
            var request = new ChangeOfProviderRequest() { ApprenticeshipHashedId = apprenticeshipHashedId };
            AssertValidationResult(x => x.ApprenticeshipHashedId, request, expectedValid);
        }

        [TestCase(default(long), false)]
        [TestCase(-2342, false)]
        [TestCase(234, true)]
        public void ThenProviderIsValidated(long providerId, bool expectedValid)
        {
            var request = new ChangeOfProviderRequest() { ProviderId = providerId };
            AssertValidationResult(x => x.ProviderId, request, expectedValid);
        }

        [TestCase(null, true)]
        [TestCase(0, false)]
        [TestCase(13, false)]
        [TestCase(1, true)]
        public void ThenNewStartMonth_IsValidated(int? newStartMonth, bool expectedValid)
        {
            var request = new ChangeOfProviderRequest() { NewStartMonth = newStartMonth };
            AssertValidationResult(x => x.NewStartMonth, request, expectedValid);
        }

        [TestCase(null, true)]
        [TestCase(0, false)]
        [TestCase(2020, true)]
        public void ThenNewStartYear_IsValidated(int? newStartYear, bool expectedValid)
        {
            var request = new ChangeOfProviderRequest() { NewStartYear = newStartYear};
            AssertValidationResult(x => x.NewStartYear, request, expectedValid);
        }

        [TestCase(null, true)]
        [TestCase(0, false)]
        [TestCase(13, false)]
        [TestCase(1, true)]
        public void ThenNewEndMonth_IsValidated(int? newEndMonth, bool expectedValid)
        {
            var request = new ChangeOfProviderRequest() { NewEndMonth = newEndMonth };
            AssertValidationResult(x => x.NewEndMonth, request, expectedValid);
        }

        [TestCase(null, true)]
        [TestCase(0, false)]
        [TestCase(2020, true)]
        public void ThenNewEndYear_IsValidated(int? newEndYear, bool expectedValid)
        {
            var request = new ChangeOfProviderRequest() { NewEndYear = newEndYear };
            AssertValidationResult(x => x.NewEndYear, request, expectedValid);
        }

        [TestCase(null, true)]
        [TestCase(0, false)]
        [TestCase(1, true)]
        [TestCase(100000, true)]
        [TestCase(100001, false)]
        public void ThenNewPrice_IsValidated(int? newPrice, bool expectedValid)
        {
            var request = new ChangeOfProviderRequest() { NewPrice= newPrice };
            AssertValidationResult(x => x.NewPrice, request, expectedValid);
        }

        private void AssertValidationResult<T>(Expression<Func<ChangeOfProviderRequest, T>> property, ChangeOfProviderRequest instance, bool expectedValid)
        {
            var validator = new ChangeOfProviderRequestValidator();

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
