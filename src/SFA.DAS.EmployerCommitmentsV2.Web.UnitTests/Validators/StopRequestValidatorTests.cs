using FluentValidation.TestHelper;
using NUnit.Framework;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using SFA.DAS.EmployerCommitmentsV2.Web.Validators;
using System;
using System.Linq.Expressions;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Validators
{
    [TestFixture]
    public class StopRequestValidatorTests
    {
        [TestCase("", false)]
        [TestCase(" ", false)]
        [TestCase("testString", true)]
        [TestCase(null, false)]
        public void ThenValidatesAccountHashedId(string accountHashedId, bool expectedValid)
        {
            var request = new StopRequest { AccountHashedId = accountHashedId };

            AssertValidationResult(x => x.AccountHashedId, request, expectedValid);
        }

        [TestCase("", false)]
        [TestCase(" ", false)]
        [TestCase("testString", true)]
        [TestCase(null, false)]
        public void ThenValidatesApprenticeshipHashedId(string apprenticeshipHashedId, bool expectedValid)
        {
            var request = new StopRequest { ApprenticeshipHashedId = apprenticeshipHashedId };

            AssertValidationResult(x => x.ApprenticeshipHashedId, request, expectedValid);
        }


        private void AssertValidationResult<T>(Expression<Func<StopRequest, T>> property, StopRequest instance, bool expectedValid)
        {
            var validator = new StopRequestValidator();

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
