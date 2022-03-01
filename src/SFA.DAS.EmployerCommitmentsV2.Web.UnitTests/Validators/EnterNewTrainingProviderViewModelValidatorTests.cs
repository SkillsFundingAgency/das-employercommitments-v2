using NUnit.Framework;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using SFA.DAS.EmployerCommitmentsV2.Web.Validators;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Validators
{
    public class EnterNewTrainingProviderViewModelValidatorTests : ValidatorTestBase<EnterNewTrainingProviderViewModel, EnterNewTrainingProviderViewModelValidator>
    {
        private const string ExpectedUkprnErrorMessage = "Select a training provider";

        [Test, MoqAutoData]
        public void WhenValidatingNewTrainingProvider_AndAnUkprnIsZero_ThenValidatorReturnsInvalid(EnterNewTrainingProviderViewModel viewModel)
        {
            viewModel.ProviderId = 0;
            AssertValidationResult(r => r.ProviderId, viewModel, false, ExpectedUkprnErrorMessage);
        }

        [Test, MoqAutoData]
        public void WhenValidatingNewTrainingProvider_AndAnUkprnIsNotGiven_ThenValidatorReturnsInvalid(EnterNewTrainingProviderViewModel viewModel)
        {
            viewModel.ProviderId = null;
            AssertValidationResult(r => r.ProviderId, viewModel, false, ExpectedUkprnErrorMessage);
        }

        [Test, MoqAutoData]
        public void WhenValidatingNewTrainingProvider_AndAnUkprnIsGiven_ThenValidatorReturnsValid(EnterNewTrainingProviderViewModel viewModel)
        {
            AssertValidationResult(r => r.ProviderId, viewModel, true);
        }

        [Test, MoqAutoData]
        public void When_ValidatingNewTrainingProvider_And_UkprnIsTheSameAsCurrentUkprn_Then_ValidatorReturnsInvalid(EnterNewTrainingProviderViewModel viewModel)
        {
            viewModel.ProviderId = 100;
            viewModel.CurrentProviderId = 100;

            AssertValidationResult(r => r.ProviderId, viewModel, false);
        }
    }
}
