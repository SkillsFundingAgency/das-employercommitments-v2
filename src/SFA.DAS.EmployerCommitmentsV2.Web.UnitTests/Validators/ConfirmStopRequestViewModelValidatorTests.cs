using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using SFA.DAS.EmployerCommitmentsV2.Web.Validators;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Validators;

[TestFixture]
public class ConfirmStopRequestViewModelValidatorTests : ValidatorTestBase<ConfirmStopRequestViewModel, ConfirmStopRequestViewModelValidator>
{
    [Test, MoqAutoData]
    public void WhenStopConfirmedIsNull_ThenValidatorReturnsInvalid(ConfirmStopRequestViewModel viewModel)
    {
        viewModel.StopConfirmed = null;

        AssertValidationResult(r => r.StopConfirmed, viewModel, false);
    }

    [Test, MoqAutoData]
    public void WhenStopConfirmedIsFalse_ThenValidatorReturnsValid(ConfirmStopRequestViewModel viewModel)
    {
        viewModel.StopConfirmed = false;

        AssertValidationResult(r => r.StopConfirmed, viewModel, true);
    }

    [Test, MoqAutoData]
    public void WhenStopConfirmedIsTrue_ThenValidatorReturnsValid(ConfirmStopRequestViewModel viewModel)
    {
        viewModel.StopConfirmed = true;

        AssertValidationResult(r => r.StopConfirmed, viewModel, true);
    }
}