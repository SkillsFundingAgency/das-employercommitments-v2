using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;
using SFA.DAS.EmployerCommitmentsV2.Web.Validators;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Validators;

[TestFixture]
public class CohortsByAccountRequestValidatorTests : ValidatorTestBase<CohortsByAccountRequest, CohortsByAccountRequestValidator>
{

    [TestCase(1, true)]
    [TestCase(default(long), false)]
    public void ThenValidatesAccountId(long accountId, bool expectedValid)
    {
        var request = new CohortsByAccountRequest { AccountId = accountId};

        AssertValidationResult(x => x.AccountId, request, expectedValid);
    }

    [TestCase("", false)]
    [TestCase(" ", false)]
    [TestCase("testString", true)]
    [TestCase(null, false)]
    public void ThenValidatesAccountHashedId(string accountHashedId, bool expectedValid)
    {
        var request = new CohortsByAccountRequest { AccountHashedId = accountHashedId};

        AssertValidationResult(x => x.AccountHashedId, request, expectedValid);
    }
}