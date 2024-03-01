using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;
using SFA.DAS.EmployerCommitmentsV2.Web.Validators;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Validators;

public class WhenValidatingConfirmProviderViewModel : ValidatorTestBase<ConfirmProviderViewModel, ConfirmProviderViewModelValidator>
{
    [Test, MoqAutoData]
    public void And_The_ViewModel_Is_Populated_Correctly_Then_The_Validator_Returns_Valid(ConfirmProviderViewModel viewModel)
    {
        viewModel.UseThisProvider = true;

        AssertValidationResult(x => x.UseThisProvider, viewModel, true);
    }

    [Test, MoqAutoData]
    public void And_The_UseThisProvider_Has_Not_Been_Set_Then_Invalid_Is_Returned(
        ConfirmProviderViewModel viewModel,
        long providerId,
        string employerAccountLegalEntityPublicHashedId,
        string providerName,
        ConfirmProviderViewModelValidator validator)
    {
        viewModel.UseThisProvider = null;
        AssertValidationResult(x => x.UseThisProvider, viewModel, false, "Select a training provider");
    }
}