using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using SFA.DAS.EmployerCommitmentsV2.Web.Validators;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Validators;

public class EditEndDateViewModelValidatorTests : ValidatorTestBase<EditEndDateViewModel, EditEndDateViewModelValidator>
{
    [TestCase(12, 2000, true)]
    [TestCase(13, 2000, false)]
    [TestCase(null, 2000, false)]
    [TestCase(1, null, false)]
    [TestCase(null, null, false)]
    public void Validate_EndDate_ShouldBeValidated(int? month, int? year, bool expectedValid)
    {
        var model = new EditEndDateViewModel { EndMonth = month, EndYear = year };
        AssertValidationResult(request => request.EndDate, model, expectedValid);
    }

    [TestCase("5143541", true)]
    [TestCase(" ", false)]
    [TestCase("", false)]
    [TestCase(null, false)]
    public void ThenAccountHashedIdIsValidated(string accountHashedId, bool expectedValid)
    {
        var viewModel = new EditEndDateViewModel() { AccountHashedId = accountHashedId };
        AssertValidationResult(x => x.AccountHashedId, viewModel, expectedValid);
    }

    [TestCase("5143541", true)]
    [TestCase(" ", false)]
    [TestCase("", false)]
    [TestCase(null, false)]
    public void ThenApprenticeshipHashedIdIsValidated(string apprenticeshipHashedId, bool expectedValid)
    {
        var viewModel = new EditEndDateViewModel() { ApprenticeshipHashedId = apprenticeshipHashedId };
        AssertValidationResult(x => x.ApprenticeshipHashedId, viewModel, expectedValid);
    }
}