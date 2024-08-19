using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;
using SFA.DAS.EmployerCommitmentsV2.Web.Validators;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Validators;

[TestFixture]
public class WhenValidatingSelectProviderViewModel : ValidatorTestBase<SelectProviderViewModel, SelectProviderViewModelValidator>
{ 
    [Test, MoqAutoData]
    public void AndTheProviderIdIsNull_ThenReturnsInvalid(SelectProviderViewModel viewModel)
    {
        viewModel.ProviderId = null;

        AssertValidationResult(x => x.ProviderId, viewModel, false);
    }

    [Test, MoqAutoData]
    public void AndTheProviderIdIsNonNumeric_ThenReturnsInvalid(SelectProviderViewModel viewModel)
    {
        viewModel.ProviderId = "abcdefghijklmnopqrstuvwxyz*!£$%^&*()_+¬`=-][#';/.,}{~@:?><";
            
        AssertValidationResult(x => x.ProviderId, viewModel, false);
    }

    [Test, MoqAutoData]
    public void AndTheProviderIdIsLessThanOne_ThenReturnsInvalid(
        SelectProviderViewModel viewModel)
    {
        viewModel.ProviderId = "-1";

        AssertValidationResult(x => x.ProviderId, viewModel, false);
    }

    [Test, MoqAutoData]
    public void AndTheProviderIdIsAboveLongMaxValue_ThenReturnsInvalid(SelectProviderViewModel viewModel)
    {
        viewModel.ProviderId = double.MaxValue.ToString();

        AssertValidationResult(x => x.ProviderId, viewModel, false);
    }

    [Test, MoqAutoData]
    public void AndTheProviderIdIsGreaterThanOne_ThenReturnsValid(SelectProviderViewModel viewModel, long providerId)
    {
        viewModel.ProviderId = Convert.ToString(providerId);

        AssertValidationResult(x => x.ProviderId, viewModel, true);
    }

    [Test, MoqAutoData]
    public void AndProviderIdIsInvalid_ThenCorrectValidationMessageShown(SelectProviderViewModel viewModel, string invalidId)
    {
        var expectedMessage = "Select a training provider";
        viewModel.ProviderId = invalidId;

        AssertValidationResult(x => x.ProviderId, viewModel, false, expectedMessage);
    }

    [Test, MoqAutoData]
    public void AndTheEmployerLegalEntityPublicHashedIdIsEmpty_ThenReturnsInvalid(SelectProviderViewModel viewModel)
    {
        viewModel.AccountLegalEntityHashedId = string.Empty;

        AssertValidationResult(x => x.AccountLegalEntityHashedId, viewModel, false);
    }

    [Test, MoqAutoData]
    public void AndTheEmployerLegalEntityPublicHashedIdIsWhiteSpace_ThenReturnsInvalid(SelectProviderViewModel viewModel)
    {
        viewModel.AccountLegalEntityHashedId = "  ";

        AssertValidationResult(x => x.AccountLegalEntityHashedId, viewModel, false);
    }

    [Test, MoqAutoData]
    public void AndViewModelIsValid_ThenReturnsValid(SelectProviderViewModel viewModel)
    {
        AssertValidationResult(x => x.AccountLegalEntityHashedId, viewModel, true);
    }
}