using NUnit.Framework;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using SFA.DAS.EmployerCommitmentsV2.Web.Validators;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Validators;

[TestFixture]
public class ConfirmStopRequestValidatorTests : ValidatorTestBase<ConfirmStopRequest, ConfirmStopRequestValidator>
{
    [TestCase("", false)]
    [TestCase(" ", false)]
    [TestCase("testString", true)]
    [TestCase(null, false)]
    public void ThenValidatesAccountHashedId(string accountHashedId, bool expectedValid)
    {
        var request = new ConfirmStopRequest { AccountHashedId = accountHashedId };

        AssertValidationResult(x => x.AccountHashedId, request, expectedValid);
    }

    [TestCase("", false)]
    [TestCase(" ", false)]
    [TestCase("testString", true)]
    [TestCase(null, false)]
    public void ThenValidatesApprenticeshipHashedId(string apprenticeshipHashedId, bool expectedValid)
    {
        var request = new ConfirmStopRequest { ApprenticeshipHashedId = apprenticeshipHashedId };

        AssertValidationResult(x => x.ApprenticeshipHashedId, request, expectedValid);
    }

    [TestCase(true, true)]
    [TestCase(false, true)]
    [TestCase(null, false)]
    public void ThenValidatesMadeRedundant(bool? madeRedundant, bool expectedValid)
    {
        var request = new ConfirmStopRequest { MadeRedundant = madeRedundant };

        AssertValidationResult(x => x.MadeRedundant, request, expectedValid);
    }
}