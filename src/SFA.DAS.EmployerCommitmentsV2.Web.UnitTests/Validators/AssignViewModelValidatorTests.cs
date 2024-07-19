using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;
using SFA.DAS.EmployerCommitmentsV2.Web.Validators;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Validators;

[TestFixture]
public class AssignViewModelValidatorTests : ValidatorTestBase<AssignViewModel, AssignViewModelValidator>
{
    [TestCase("5143541", true)]
    [TestCase(" ", true)]
    [TestCase("", true)]
    [TestCase("&", true)]
    [TestCase("-", true)]
    [TestCase("/", true)]
    [TestCase(">", false)]
    [TestCase("|", false)]
    public void ThenAccountHashedIdIsValidated(string message, bool expectedValid)
    {
        var request = new AssignViewModel {Message = message, WhoIsAddingApprentices = WhoIsAddingApprentices.Provider };
        AssertValidationResult(x => x.Message, request, expectedValid);
    }
}