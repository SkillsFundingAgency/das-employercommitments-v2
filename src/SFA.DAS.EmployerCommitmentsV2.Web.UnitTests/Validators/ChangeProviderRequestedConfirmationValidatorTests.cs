using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using SFA.DAS.EmployerCommitmentsV2.Web.Validators;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Validators;

[TestFixture]
public class ChangeProviderRequestedConfirmationValidatorTests : ValidatorTestBase<ChangeProviderRequestedConfirmationRequest, ChangeProviderRequestedConfirmationValidator>
{
    [TestCase(1, true)]
    [TestCase(default(long), false)]
    public void ThenValidatesProviderId(long providerId, bool expectedValid)
    {
        var request = new ChangeProviderRequestedConfirmationRequest { ProviderId = providerId};

        AssertValidationResult(x => x.ProviderId, request, expectedValid);
    }

    [TestCase(1, true)]
    [TestCase(default(long), false)]
    public void ThenValidatesApprenticeshipId(long apprenticeshipId, bool expectedValid)
    {
        var request = new ChangeProviderRequestedConfirmationRequest { ApprenticeshipId = apprenticeshipId};

        AssertValidationResult(x => x.ApprenticeshipId, request, expectedValid);
    }

    [TestCase("", false)]
    [TestCase(" ", false)]
    [TestCase("testString", true)]
    [TestCase(null, false)]
    public void ThenValidatesAccountHashedId(string accountHashedId, bool expectedValid)
    {
        var request = new ChangeProviderRequestedConfirmationRequest { AccountHashedId = accountHashedId };

        AssertValidationResult(x => x.AccountHashedId, request, expectedValid);
    }

    [TestCase("", false)]
    [TestCase(" ", false)]
    [TestCase("testString", true)]
    [TestCase(null, false)]
    public void ThenValidatesApprenticeshipHashedId(string apprenticeshipHashedId, bool expectedValid)
    {
        var request = new ChangeProviderRequestedConfirmationRequest { ApprenticeshipHashedId = apprenticeshipHashedId };

        AssertValidationResult(x => x.ApprenticeshipHashedId, request, expectedValid);
    }
}