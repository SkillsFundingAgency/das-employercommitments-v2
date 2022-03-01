using NUnit.Framework;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using SFA.DAS.EmployerCommitmentsV2.Web.Validators;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Validators
{
    [TestFixture]
    public class PauseRequestViewModelValidatorTests : ValidatorTestBase<PauseRequestViewModel, PauseRequestViewModelValidator>
    {
        [Test, MoqAutoData]
        public void WhenPauseConfirmedIsNull_ThenValidatorReturnsInvalid(PauseRequestViewModel viewModel)
        {
            viewModel.PauseConfirmed = null;

            AssertValidationResult(x => x.PauseConfirmed, viewModel, false);
        }

        [Test, MoqAutoData]
        public void WhenPauseConfirmedIsFalse_ThenValidatorReturnsValid(PauseRequestViewModel viewModel)
        {
            viewModel.PauseConfirmed = false;

            AssertValidationResult(x => x.PauseConfirmed, viewModel, true);
        }

        [Test, MoqAutoData]
        public void WhenPauseConfirmedIsTrue_ThenValidatorReturnsValid(PauseRequestViewModel viewModel)
        {
            viewModel.PauseConfirmed = true;

            AssertValidationResult(x => x.PauseConfirmed, viewModel, true);
        }
    }
}
