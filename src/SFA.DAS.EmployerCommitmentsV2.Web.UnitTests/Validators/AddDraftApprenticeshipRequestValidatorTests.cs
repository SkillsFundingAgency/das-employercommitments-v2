using System;
using System.Linq.Expressions;
using FluentValidation.TestHelper;
using NUnit.Framework;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.DraftApprenticeship;
using SFA.DAS.EmployerCommitmentsV2.Web.Validators;
using SFA.DAS.Testing;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Validators
{
    [TestFixture]
    [Parallelizable]
    public class AddDraftApprenticeshipRequestValidatorTests : FluentTest<AddDraftApprenticeshipRequestValidatorTestsFixture>
    {
        [TestCase(null, false)]
        [TestCase("", false)]
        [TestCase("AAA111", true)]
        public void Validate_WhenValidatingAccountHashedId_ThenShouldBeValidated(string accountHashedId, bool isValid)
        {
            Test(
                f => f.Request.AccountHashedId = accountHashedId,
                f => f.Verify(r => r.AccountHashedId, isValid));
        }
        
        [TestCase(null, false)]
        [TestCase("", false)]
        [TestCase("AAA111", true)]
        public void Validate_WhenValidatingCohortReference_ThenShouldBeValidated(string cohortReference, bool isValid)
        {
            Test(
                f => f.Request.CohortReference = cohortReference,
                f => f.Verify(r => r.CohortReference, isValid));
        }
        
        [TestCase(-1, false)]
        [TestCase(0, false)]
        [TestCase(1, true)]
        public void Validate_WhenValidatingCohortId_ThenShouldBeValidated(long accountLegalEntityId, bool isValid)
        {
            Test(
                f => f.Request.CohortId = accountLegalEntityId,
                f => f.Verify(r => r.CohortId, isValid));
        }
        
        [TestCase(null, false)]
        [TestCase("", false)]
        [TestCase("AAA111", true)]
        public void Validate_WhenValidatingAccountLegalEntityHashedId_ThenShouldBeValidated(string accountLegalEntityHashedId, bool isValid)
        {
            Test(
                f => f.Request.AccountLegalEntityHashedId = accountLegalEntityHashedId,
                f => f.Verify(r => r.AccountLegalEntityHashedId, isValid));
        }
        
        [TestCase(-1, false)]
        [TestCase(0, false)]
        [TestCase(1, true)]
        public void Validate_WhenValidatingAccountLegalEntityId_ThenShouldBeValidated(long accountLegalEntityId, bool isValid)
        {
            Test(
                f => f.Request.AccountLegalEntityId = accountLegalEntityId,
                f => f.Verify(r => r.AccountLegalEntityId, isValid));
        }
        
        [TestCase(false, false)]
        [TestCase(true, true)]
        public void Validate_WhenValidatingReservationId_ThenShouldBeValidated(bool reservationIdHasValue, bool isValid)
        {
            Test(
                f => f.Request.ReservationId = reservationIdHasValue ? Guid.NewGuid() : Guid.Empty,
                f => f.Verify(r => r.ReservationId, isValid));
        }
        
        [TestCase(null, true)]
        [TestCase("", true)]
        [TestCase("Foobar", false)]
        [TestCase("082019", true)]
        [TestCase("01082019", false)]
        public void Validate_WhenValidatingStartMonthYear_ThenShouldBeValidated(string startMonthYear, bool isValid)
        {
            Test(
                f => f.Request.StartMonthYear = startMonthYear,
                f => f.Verify(r => r.StartMonthYear, isValid));
        }
    }

    public class AddDraftApprenticeshipRequestValidatorTestsFixture
    {
        public AddDraftApprenticeshipRequest Request { get; set; }
        public AddDraftApprenticeshipRequestValidator Validator { get; set; }

        public AddDraftApprenticeshipRequestValidatorTestsFixture()
        {
            Request = new AddDraftApprenticeshipRequest();
            Validator = new AddDraftApprenticeshipRequestValidator();
        }

        public void Verify<T>(Expression<Func<AddDraftApprenticeshipRequest, T>> property, bool isValid)
        {
            if (isValid)
            {
                Validator.ShouldNotHaveValidationErrorFor(property, Request);
            }
            else
            {
                Validator.ShouldHaveValidationErrorFor(property, Request);
            }
        }
    }
}