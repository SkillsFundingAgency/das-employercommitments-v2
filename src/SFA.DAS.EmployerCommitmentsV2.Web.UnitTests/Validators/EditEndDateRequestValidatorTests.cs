﻿using FluentValidation.TestHelper;
using NUnit.Framework;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using SFA.DAS.EmployerCommitmentsV2.Web.Validators;
using System;
using System.Linq.Expressions;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Validators
{
    public class EditEndDateRequestValidatorTests
    {
        [TestCase("5143541", true)]
        [TestCase(" ", false)]
        [TestCase("", false)]
        [TestCase(null, false)]
        public void ThenAccountHashedIdIsValidated(string accountHashedId, bool expectedValid)
        {
            var request = new EditEndDateRequest() { AccountHashedId = accountHashedId };
            AssertValidationResult(x => x.AccountHashedId, request, expectedValid);
        }

        [TestCase("5143541", true)]
        [TestCase(" ", false)]
        [TestCase("", false)]
        [TestCase(null, false)]
        public void ThenApprenticeshipHashedIdIsValidated(string apprenticeshipHashedId, bool expectedValid)
        {
            var request = new EditEndDateRequest() { ApprenticeshipHashedId = apprenticeshipHashedId };
            AssertValidationResult(x => x.ApprenticeshipHashedId, request, expectedValid);
        }

        private void AssertValidationResult<T>(Expression<Func<EditEndDateRequest, T>> property, EditEndDateRequest instance, bool expectedValid)
        {
            var validator = new EditEndDateRequestValidator();

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
