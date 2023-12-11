using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using SFA.DAS.EmployerCommitmentsV2.Web.Validators;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Validators;

[TestFixture]
public class MadeRedundantViewModelValidatorTests : ValidatorTestBase<MadeRedundantViewModel, MadeRedundantViewModelValidator>
{
    [Test, MoqAutoData]
    public void WhenMadeRedundantConfirmedIsNull_ThenValidatorReturnsInvalid(MadeRedundantViewModel viewModel)
    {
        viewModel.MadeRedundant = null;

        AssertValidationResult(r => r.MadeRedundant, viewModel, false);
    }

    [Test, MoqAutoData]
    public void WhenMadeRedundantConfirmedIsFalse_ThenValidatorReturnsValid(MadeRedundantViewModel viewModel)
    {
        viewModel.MadeRedundant = false;

        AssertValidationResult(r => r.MadeRedundant, viewModel, true);
    }

    [Test, MoqAutoData]
    public void WhenMadeRedundantConfirmedIsTrue_ThenValidatorReturnsValid(MadeRedundantViewModel viewModel)
    {
        viewModel.MadeRedundant = true;

        AssertValidationResult(r => r.MadeRedundant, viewModel, true);
    }
}