using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using FluentValidation;
using FluentValidation.TestHelper;
using FluentValidation.Validators;
using Microsoft.WindowsAzure.Storage.Table.Protocol;
using Newtonsoft.Json.Serialization;
using NUnit.Framework;
using Remotion.Linq.Clauses;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.DraftApprenticeship;
using SFA.DAS.EmployerCommitmentsV2.Web.Validators;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Validators
{
    [TestFixture]
    public class DeleteDraftApprenticeshipRequestTests
    {
        [TestCase("5143541", true)]
        [TestCase(" ", false)]
        [TestCase("", false)]
        [TestCase(null, false)]
        public void ThenAccountHashedIdIsValidated(string accountHashedId, bool expectedValid)
        {
            var request = new DeleteApprenticeshipRequest() {AccountHashedId = accountHashedId};
            AssertValidationResult(x => x.AccountHashedId, request, expectedValid);
        }
        [TestCase("", false)]
        [TestCase("31", true)]
        [TestCase(" ", false)]
        [TestCase(null, false)]
        public void ThenCohortReferenceIsValidated(string cohortReference, bool expectedValid)
        {
            var request = new DeleteApprenticeshipRequest() {CohortReference = cohortReference};
            AssertValidationResult(x => x.CohortReference,request, expectedValid);
        }

        [TestCase(default(long), false)]
        [TestCase(-2342, false)]
        [TestCase(234, true)]
        public void ThenCohortIdIsValidated(long cohortReference, bool expectedValid)
        {
            var request = new DeleteApprenticeshipRequest() { CohortId = cohortReference };
            AssertValidationResult(x => x.CohortId, request, expectedValid);
        }

        [TestCase(default(long), false)]
        [TestCase(-2342, false)]
        [TestCase(234, true)]
        public void ThenDraftApprenticeshipIdIsValidated(long draftApprenticeshipId, bool expectedValid)
        {
            var request = new DeleteApprenticeshipRequest() { DraftApprenticeshipId = draftApprenticeshipId};
            AssertValidationResult(x => x.DraftApprenticeshipId, request, expectedValid);
        }

        //[TestCase(null, false)]
        //[TestCase(" ", false)]
        //[TestCase("1", true)]
        //public void ThenOriginIsValidated(string origin, bool expectedValid)
        //{
        //    var request = new DeleteDraftApprenticeshipRequest() { Origin = origin };
        //    AssertValidationResult(x => x.Origin, request, expectedValid);
        //}

        private void AssertValidationResult<T>(Expression<Func<DeleteApprenticeshipRequest, T>> property, DeleteApprenticeshipRequest instance, bool expectedValid)
        {
            var validator = new DeleteDraftApprenticeshipRequestValidator();

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
