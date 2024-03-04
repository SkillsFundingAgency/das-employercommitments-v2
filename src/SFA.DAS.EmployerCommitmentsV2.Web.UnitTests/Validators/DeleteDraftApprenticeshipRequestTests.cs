using SFA.DAS.EmployerCommitmentsV2.Web.Models.DraftApprenticeship;
using SFA.DAS.EmployerCommitmentsV2.Web.Validators;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Validators;

[TestFixture]
public class DeleteDraftApprenticeshipRequestTests : ValidatorTestBase<DeleteApprenticeshipRequest, DeleteDraftApprenticeshipRequestValidator>
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
}