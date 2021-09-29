using NUnit.Framework;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using SFA.DAS.EmployerCommitmentsV2.Web.Validators;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Validators
{
    [TestFixture]
    public class ChangeOptionViewModelValidatorTests
    {
        [Test, MoqAutoData]
        public void When_SelectedOptionIsNull_Then_ReturnInvalid(
               ChangeOptionViewModel viewModel,
               ChangeOptionViewModelValidator validator)
        {
            viewModel.SelectedOption = null;

            var result = validator.Validate(viewModel);

            Assert.False(result.IsValid);
        }

        [Test, MoqAutoData]
        public void When_OnlyChangingOption_And_CurrentOptionIsSelected_Then_ReturnInvalid(
            ChangeOptionViewModel viewModel,
            ChangeOptionViewModelValidator validator)
        {
            viewModel.ReturnToEdit = false;
            viewModel.ReturnToChangeVersion = false;
            viewModel.SelectedOption = viewModel.CurrentOption;

            var result = validator.Validate(viewModel);

            Assert.False(result.IsValid);
        }
    }
}
