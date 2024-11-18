using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;
using SFA.DAS.EmployerCommitmentsV2.Web.Validators;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Validators;

public class SelectFundingViewModelValidatorTests : ValidatorTestBase<SelectFundingViewModel, SelectFundingViewModelValidator>
{
    [Test]
    public void WhenNoOptionIsSelected_ThenValidatorReturnsInvalid()
    {

        var viewModel = new SelectFundingViewModel() { FundingType = null };

        AssertValidationResult(x => x.FundingType, viewModel, false);
    }
}