using System;
using System.Linq.Expressions;
using FluentValidation.TestHelper;
using NUnit.Framework;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;
using SFA.DAS.EmployerCommitmentsV2.Web.Validators;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Validators
{
    [TestFixture]
    public class ReviewRequestValidatorTest
    {

        [TestCase(1, true)]
        [TestCase(default(long), false)]
        public void ThenValidatesAccountId(long accountId, bool expectedValid)
        {
            var request = new ReviewRequest { AccountId = accountId };

            AssertValidationResult(x => x.AccountId, request, expectedValid);
        }

        [TestCase("", false)]
        [TestCase(" ", false)]
        [TestCase("testString", true)]
        [TestCase(null, false)]
        public void ThenValidatesAccountHashedId(string accountHashedId, bool expectedValid)
        {
            var request = new ReviewRequest { AccountHashedId = accountHashedId };

            AssertValidationResult(x => x.AccountHashedId, request, expectedValid);
        }


        private void AssertValidationResult<T>(Expression<Func<ReviewRequest, T>> property, ReviewRequest instance, bool expectedValid)
        {
            var validator = new ReviewRequestValidator();

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
