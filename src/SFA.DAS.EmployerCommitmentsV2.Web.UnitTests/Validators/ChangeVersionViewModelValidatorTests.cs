using NUnit.Framework;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using SFA.DAS.EmployerCommitmentsV2.Web.Validators;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Validators
{
    [TestFixture]
    public class ChangeVersionViewModelValidatorTests : ValidatorTestBase<ChangeVersionViewModel, ChangeVersionViewModelValidator>
    {
        [Test, MoqAutoData]
        public void When_SelectedVersionIsNull_Then_ReturnInvalid(
            ChangeVersionViewModel viewModel)
        {
            viewModel.SelectedVersion = null;

            AssertValidationResult(r => r.SelectedVersion, viewModel, false);
        }
    }
}
