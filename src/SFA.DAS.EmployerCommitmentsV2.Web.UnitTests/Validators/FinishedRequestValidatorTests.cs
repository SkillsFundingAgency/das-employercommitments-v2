using System;
using System.Linq.Expressions;
using FluentValidation.TestHelper;
using NUnit.Framework;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;
using SFA.DAS.EmployerCommitmentsV2.Web.Validators;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Validators;

[TestFixture]
public class FinishedRequestValidatorTests : ValidatorTestBase<FinishedRequest, FinishedRequestValidator>
{
    [TestCase(null, false)]
    [TestCase("", false)]
    [TestCase("AAA111", true)]
    public void Validate_ProviderId_ShouldBeValidated(string accountHashedId, bool expectToBeValid)
    {
        var model = new FinishedRequest { AccountHashedId = accountHashedId };
        AssertValidationResult(request => request.AccountHashedId, model, expectToBeValid);
    }
        
    [TestCase(-1, false)]
    [TestCase(0, false)]
    [TestCase(1, true)]
    public void Validate_AccountId_ShouldBeValidated(long cohortId, bool expectToBeValid)
    {
        var model = new FinishedRequest { AccountId = cohortId };
        AssertValidationResult(request => request.AccountId, model, expectToBeValid);
    }

    [TestCase(null, false)]
    [TestCase("", false)]
    [TestCase("AAA111", true)]
    public void Validate_StartDate_ShouldBeValidated(string cohortReference, bool expectedValid)
    {
        var model = new FinishedRequest { CohortReference = cohortReference };
        AssertValidationResult(request => request.CohortReference, model, expectedValid);
    }
        
    [TestCase(-1, false)]
    [TestCase(0, false)]
    [TestCase(1, true)]
    public void Validate_CohortId_ShouldBeValidated(long cohortId, bool expectToBeValid)
    {
        var model = new FinishedRequest { CohortId = cohortId };
        AssertValidationResult(request => request.CohortId, model, expectToBeValid);
    }
}