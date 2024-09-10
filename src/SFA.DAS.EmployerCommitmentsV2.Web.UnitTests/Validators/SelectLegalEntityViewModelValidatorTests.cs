using SFA.DAS.EmployerCommitmentsV2.Web.Validators;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Validators;

public class SelectLegalEntityViewModelValidatorTests : ValidatorTestBase<SelectLegalEntityViewModel, SelectLegalEntityViewModelValidator>
{
    [Test]
    public void WhenNoOptionIsSelected_ThenValidatorReturnsInvalid()
    {

        var viewModel = new SelectLegalEntityViewModel { LegalEntityId = 0 };

        AssertValidationResult(x => x.LegalEntityId, viewModel, false, "Select which organisation is named on the contract with the training provider");
    }
}