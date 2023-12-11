using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using SFA.DAS.EmployerCommitmentsV2.Web.Validators;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Validators;

[TestFixture]
public class WhoWillEnterTheDetailsViewModelValidatorTests : ValidatorTestBase<WhoWillEnterTheDetailsViewModel, WhoWillEnterTheDetailsViewModelValidator>
{
    private const string errorMessage = "Select who will enter the new course dates and price";

    [Test]
    [MoqAutoData]
    public void WhenValidatingWhoWillEnterTheDetails_AndSelectionIsNotMade_ThenValidatorReturnsInvalid(WhoWillEnterTheDetailsViewModel viewModel)
    {
        viewModel.EmployerWillAdd = null;

        AssertValidationResult(x => x.EmployerWillAdd, viewModel, false, errorMessage);
    }

    [Test]
    [InlineAutoData(true)]
    [InlineAutoData(false)]
    public void WhenValidatingWhoWillEnterTheDetails_AndSelectionIsMade_ThenValidatorReturnsValid(bool employerResponsibility, WhoWillEnterTheDetailsViewModel viewModel)
    {
        viewModel.EmployerWillAdd = employerResponsibility;
        AssertValidationResult(x => x.EmployerWillAdd, viewModel, true);
    }
}