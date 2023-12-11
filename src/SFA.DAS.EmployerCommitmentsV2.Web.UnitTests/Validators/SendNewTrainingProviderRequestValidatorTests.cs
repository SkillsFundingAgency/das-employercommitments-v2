using NUnit.Framework;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using SFA.DAS.EmployerCommitmentsV2.Web.Validators;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Validators;

[TestFixture]
public class SendNewTrainingProviderRequestValidatorTests : ValidatorTestBase<SendNewTrainingProviderRequest, SendNewTrainingProviderRequestValidator>
{
    [TestCase("5143541", true)]
    [TestCase(" ", false)]
    [TestCase("", false)]
    [TestCase(null, false)]
    public void ThenAccountHashedIdIsValidated(string accountHashedId, bool expectedValid)
    {
        var request = new SendNewTrainingProviderRequest() { AccountHashedId = accountHashedId };
        AssertValidationResult(x => x.AccountHashedId, request, expectedValid);
    }
    [TestCase("", false)]
    [TestCase("31", true)]
    [TestCase(" ", false)]
    [TestCase(null, false)]
    public void ThenApprenticeshipHashedIdIsValidated(string apprenticeshipHashedId, bool expectedValid)
    {
        var request = new SendNewTrainingProviderRequest() { ApprenticeshipHashedId = apprenticeshipHashedId };
        AssertValidationResult(x => x.ApprenticeshipHashedId, request, expectedValid);
    }

    [TestCase(default(long), false)]
    [TestCase(-2342, false)]
    [TestCase(234, true)]
    public void ThenProviderIsValidated(long providerId, bool expectedValid)
    {
        var request = new SendNewTrainingProviderRequest() { ProviderId = providerId };
        AssertValidationResult(x => x.ProviderId, request, expectedValid);
    }

    [TestCase(default(long), false)]
    [TestCase(-2342, false)]
    [TestCase(234, true)]
    public void ThenApprenticeshipIdIsValidated(long apprenticeshipId, bool expectedValid)
    {
        var request = new SendNewTrainingProviderRequest() { ApprenticeshipId = apprenticeshipId };
        AssertValidationResult(x => x.ApprenticeshipId, request, expectedValid);
    }

    [TestCase(default(long), false)]
    [TestCase(-2342, false)]
    [TestCase(234, true)]
    public void ThenAccountIdIsValidated(long accountId, bool expectedValid)
    {
        var request = new SendNewTrainingProviderRequest() { AccountId = accountId };
        AssertValidationResult(x => x.AccountId, request, expectedValid);
    }
}