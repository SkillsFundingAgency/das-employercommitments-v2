using NUnit.Framework;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using SFA.DAS.EmployerCommitmentsV2.Web.Validators;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Validators;

[TestFixture]
public class ChangeOptionViewModelValidatorTests : ValidatorTestBase<ChangeOptionViewModel, ChangeOptionViewModelValidator>
{
    [Test, MoqAutoData]
    public void When_SelectedOptionIsNull_Then_ReturnInvalid(
        ChangeOptionViewModel viewModel)
    {
        viewModel.SelectedOption = null;

        AssertValidationResult(r => r.SelectedOption, viewModel, false);
    }

    [Test, MoqAutoData]
    public void When_OnlyChangingOption_And_CurrentOptionIsSelected_Then_ReturnInvalid(
        ChangeOptionViewModel viewModel,
        ChangeOptionViewModelValidator validator)
    {
        viewModel.ReturnToEdit = false;
        viewModel.ReturnToChangeVersion = false;
        viewModel.SelectedOption = viewModel.CurrentOption;

        AssertValidationResult(r => r.SelectedOption, viewModel, false);
    }
}