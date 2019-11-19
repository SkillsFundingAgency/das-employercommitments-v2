using System;
using System.Linq.Expressions;
using FluentValidation.TestHelper;
using NUnit.Framework;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;
using SFA.DAS.EmployerCommitmentsV2.Web.Validators;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Validators
{
    [TestFixture]
    public class DraftRequestValidatorTests
    {

        [TestCase(1, true)]
        [TestCase(default(long), false)]
        public void ThenValidatesAccountId(long accountId, bool expectedValid)
        {
            var request = new DraftRequest {AccountId = accountId};

            AssertValidationResult(x => x.AccountId, request, expectedValid);
        }

        [TestCase("", false)]
        [TestCase(" ", false)]
        [TestCase("testString", true)]
        [TestCase(null, false)]
        public void ThenValidatesAccountHashedId(string accountHashedId, bool expectedValid)
        {
            var request = new DraftRequest { AccountHashedId = accountHashedId};

            AssertValidationResult(x => x.AccountHashedId, request, expectedValid);
        }


        private void AssertValidationResult<T>(Expression<Func<DraftRequest, T>> property, DraftRequest instance, bool expectedValid)
        {
            var validator = new DraftRequestValidator();

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
