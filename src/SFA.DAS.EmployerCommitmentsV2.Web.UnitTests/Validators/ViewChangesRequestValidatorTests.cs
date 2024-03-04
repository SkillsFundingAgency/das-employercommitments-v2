using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using SFA.DAS.EmployerCommitmentsV2.Web.Validators;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Validators;

[TestFixture]
public class ViewChangesRequestValidatorTests : ValidatorTestBase<ViewChangesRequest, ViewChangesRequestValidator>
{
    [TestCase("5143541", true)]
    [TestCase(" ", false)]
    [TestCase("", false)]
    [TestCase(null, false)]
    public void ThenAccountHashedIdIsValidated(string accountHashedId, bool expectedValid)
    {
        var request = new ViewChangesRequest { AccountHashedId = accountHashedId };
        AssertValidationResult(x => x.AccountHashedId, request, expectedValid);
    }

    [TestCase("", false)]
    [TestCase("31", true)]
    [TestCase(" ", false)]
    [TestCase(null, false)]
    public void ThenApprenticeshipHashedIdIsValidated(string apprenticeshipHashedId, bool expectedValid)
    {
        var request = new ViewChangesRequest { ApprenticeshipHashedId = apprenticeshipHashedId };
        AssertValidationResult(x => x.ApprenticeshipHashedId, request, expectedValid);
    }

    [TestCase(0, false)]
    [TestCase(123456, true)]
    public void ThenApprenticeshipIdIsValidated(long apprenticeshipId, bool expectedValid)
    {
        var request = new ViewChangesRequest { ApprenticeshipId = apprenticeshipId };
        AssertValidationResult(x => x.ApprenticeshipId, request, expectedValid);
    }
}