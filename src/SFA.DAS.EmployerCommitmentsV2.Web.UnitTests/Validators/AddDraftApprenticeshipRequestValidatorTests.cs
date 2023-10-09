using System;
using NUnit.Framework;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.DraftApprenticeship;
using SFA.DAS.EmployerCommitmentsV2.Web.Validators;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Validators
{
    [TestFixture]
    [Parallelizable]
    public class AddDraftApprenticeshipRequestValidatorTests : ValidatorTestBase<AddDraftApprenticeshipRequest, AddDraftApprenticeshipRequestValidator>
    {
        [TestCase(null, false)]
        [TestCase("", false)]
        [TestCase("AAA111", true)]
        public void Validate_WhenValidatingAccountHashedId_ThenShouldBeValidated(string accountHashedId, bool isValid)
        {
            var model = new AddDraftApprenticeshipRequest { AccountHashedId = accountHashedId };
            AssertValidationResult(request => request.AccountHashedId, model, isValid);
        }
        
        [TestCase(null, false)]
        [TestCase("", false)]
        [TestCase("AAA111", true)]
        public void Validate_WhenValidatingCohortReference_ThenShouldBeValidated(string cohortReference, bool isValid)
        {
            var model = new AddDraftApprenticeshipRequest { CohortReference = cohortReference };
            AssertValidationResult(request => request.CohortReference, model, isValid);
        }
        
        [TestCase(-1, false)]
        [TestCase(0, false)]
        [TestCase(1, true)]
        public void Validate_WhenValidatingCohortId_ThenShouldBeValidated(long cohortId, bool isValid)
        {
            var model = new AddDraftApprenticeshipRequest { CohortId = cohortId };
            AssertValidationResult(request => request.CohortId, model, isValid);
        }
        
        [TestCase(null, false)]
        [TestCase("", false)]
        [TestCase("AAA111", true)]
        public void Validate_WhenValidatingAccountLegalEntityHashedId_ThenShouldBeValidated(string accountLegalEntityHashedId, bool isValid)
        {
            var model = new AddDraftApprenticeshipRequest { AccountLegalEntityHashedId = accountLegalEntityHashedId };
            AssertValidationResult(request => request.AccountLegalEntityHashedId, model, isValid);
        }
        
        [TestCase(-1, false)]
        [TestCase(0, false)]
        [TestCase(1, true)]
        public void Validate_WhenValidatingAccountLegalEntityId_ThenShouldBeValidated(long accountLegalEntityId, bool isValid)
        {
            var model = new AddDraftApprenticeshipRequest { AccountLegalEntityId = accountLegalEntityId };
            AssertValidationResult(request => request.AccountLegalEntityId, model, isValid);
        }
        
        [TestCase(false, false)]
        [TestCase(true, true)]
        public void Validate_WhenValidatingReservationId_ThenShouldBeValidated(bool reservationIdHasValue, bool isValid)
        {
            var model = new AddDraftApprenticeshipRequest { ReservationId = reservationIdHasValue ? Guid.NewGuid() : Guid.Empty };
            AssertValidationResult(request => request.ReservationId, model, isValid);
        }
        
        [TestCase(null, true)]
        [TestCase("", true)]
        [TestCase("Foobar", false)]
        [TestCase("082019", true)]
        [TestCase("01082019", false)]
        public void Validate_WhenValidatingStartMonthYear_ThenShouldBeValidated(string startMonthYear, bool isValid)
        {
            var model = new AddDraftApprenticeshipRequest { StartMonthYear = startMonthYear };
            AssertValidationResult(request => request.StartMonthYear, model, isValid);
        }
    }
}