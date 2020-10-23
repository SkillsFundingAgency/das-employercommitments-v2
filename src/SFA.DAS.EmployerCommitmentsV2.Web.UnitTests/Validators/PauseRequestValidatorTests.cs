using FluentValidation.TestHelper;
using NUnit.Framework;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using SFA.DAS.EmployerCommitmentsV2.Web.Validators;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Validators
{
    [TestFixture]
    public class PauseRequestValidatorTests
    {
        [TestCase("", false)]
        [TestCase(" ", false)]
        [TestCase("testString", true)]
        [TestCase(null, false)]
        public void ThenValidatesAccountHashedId(string accountHashedId, bool expectedValid)
        {
            var request = new PauseRequest { AccountHashedId = accountHashedId };

            AssertValidationResult(x => x.AccountHashedId, request, expectedValid);
        }

        [TestCase("", false)]
        [TestCase(" ", false)]
        [TestCase("testString", true)]
        [TestCase(null, false)]
        public void ThenValidatesApprenticeshipHashedId(string apprenticeshipHashedId, bool expectedValid)
        {
            var request = new PauseRequest { ApprenticeshipHashedId = apprenticeshipHashedId };

            AssertValidationResult(x => x.ApprenticeshipHashedId, request, expectedValid);
        }


        private void AssertValidationResult<T>(Expression<Func<PauseRequest, T>> property, PauseRequest instance, bool expectedValid)
        {
            var validator = new PauseRequestValidator();

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
